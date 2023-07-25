using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LogicAndTrick.Oy;
using Microsoft.Win32;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Documents;
using CBRE.Common.Shell.Hooks;
using CBRE.Common.Shell.Settings;
using CBRE.Common.Threading;

namespace CBRE.Shell.Registers
{
    /// <summary>
    /// The document register handles document loaders
    /// </summary>
    [Export(typeof(IStartupHook))]
    [Export(typeof(ISettingsContainer))]
    [Export]
    public class DocumentRegister : IStartupHook, ISettingsContainer
    {
        private readonly ThreadSafeList<IDocument> _openDocuments;
        public IReadOnlyCollection<IDocument> OpenDocuments => _openDocuments;

        private readonly List<IDocumentLoader> _loaders;

        private readonly string _programId;
        private readonly string _programIdVer = "1";

        [ImportingConstructor]
        public DocumentRegister(
            [ImportMany] IEnumerable<Lazy<IDocumentLoader>> documentLoaders
        )
        {
            _loaders = documentLoaders.Select(x => x.Value).ToList();

            string assembly = Assembly.GetEntryAssembly()?.GetName().Name ?? "CBRE.Shell";
            _programId = assembly.Replace(".", "");

            _openDocuments = new ThreadSafeList<IDocument>();
        }

        public Task OnStartup()
        {
            RegisterExtensionHandlers();
            return Task.FromResult(0);
        }

        // Public interface

        public IEnumerable<FileExtensionInfo> GetSupportedFileExtensions(IDocument document)
        {
            return _loaders.Where(x => x.CanSave(document)).SelectMany(x => x.SupportedFileExtensions);
        }

        public DocumentPointer GetDocumentPointer(IDocument document)
        {
            IDocumentLoader loader = _loaders.FirstOrDefault(x => x.CanSave(document));
            DocumentPointer pointer = loader?.GetDocumentPointer(document);
            return pointer;
        }

        // Save/load/open documents

        public bool IsOpen(string fileName)
        {
            return _openDocuments.Any(x => string.Equals(x.FileName, fileName, StringComparison.InvariantCultureIgnoreCase));
        }

        public IDocument GetDocumentByFileName(string fileName)
        {
            return _openDocuments.FirstOrDefault(x => string.Equals(x.FileName, fileName, StringComparison.InvariantCultureIgnoreCase));
        }

        public bool IsOpen(IDocument document)
        {
            return _openDocuments.Contains(document);
        }

        public async Task<IDocument> NewDocument(IDocumentLoader loader)
        {
            IDocument doc = await loader.CreateBlank();
            if (doc != null) OpenDocument(doc);
            return doc;
        }

        public async Task<IDocument> OpenDocument(DocumentPointer documentPointer, string loaderName = null)
        {
            string fileName = documentPointer.FileName;
            if (!File.Exists(fileName)) return null;

            if (IsOpen(fileName))
            {
                ActivateDocument(GetDocumentByFileName(fileName));
                return null;
            }

            IDocumentLoader loader = _loaders.FirstOrDefault(x => x.GetType().Name == loaderName && x.CanLoad(fileName));
            if (loader == null) return null;

            IDocument doc = await loader.Load(documentPointer);
            if (doc != null) OpenDocument(doc);

            return doc;
        }

        public async Task<IDocument> OpenDocument(string fileName, string loaderHint = "")
        {
            if (!File.Exists(fileName)) return null;

            if (IsOpen(fileName))
            {
                ActivateDocument(GetDocumentByFileName(fileName));
                return null;
            }

            IDocumentLoader loader = null;
            if (!string.IsNullOrWhiteSpace(loaderHint)) loader = _loaders.FirstOrDefault(x => x.GetType().Name == loaderHint);
            if (loader == null) loader = _loaders.FirstOrDefault(x => x.CanLoad(fileName));
            
            if (loader != null)
            {
                IDocument doc = await loader.Load(fileName);
                if (doc != null) OpenDocument(doc);
                return doc;
            }

            return null;
        }

        public async Task ActivateDocument(IDocument document)
        {
            if (document == null)
            {
                await Oy.Publish<IDocument>("Document:Activated", new NoDocument());
                await Oy.Publish("Context:Remove", new ContextInfo("ActiveDocument"));
            }
            else
            {
                await Oy.Publish("Document:Activated", document);
                await Oy.Publish("Context:Add", new ContextInfo("ActiveDocument", document));
            }
        }

        public Task<bool> ExportDocument(IDocument document, string fileName, string loaderHint = "")
        {
            return SaveDocument(document, fileName, loaderHint, false);
        }

        public Task<bool> SaveDocument(IDocument document, string fileName, string loaderHint = "")
        {
            return SaveDocument(document, fileName, loaderHint, true);
        }

        private async Task<bool> SaveDocument(IDocument document, string fileName, string loaderHint, bool switchFileName)
        {
            if (document == null || fileName == null) return false;

            IDocumentLoader loader = null;
            if (!string.IsNullOrWhiteSpace(loaderHint)) loader = _loaders.FirstOrDefault(x => x.GetType().Name == loaderHint);
            if (loader == null) loader = _loaders.FirstOrDefault(x => x.CanSave(document) && x.CanLoad(fileName));

            if (loader == null) return false;

            await Oy.Publish("Document:BeforeSave", document);

            await loader.Save(document, fileName);

            // Only publish document saved when the file name is changed
            // Otherwise we're not actually saving the document's file
            if (switchFileName)
            {
                document.FileName = fileName;
                await Oy.Publish("Document:Saved", document);
            }
            
            return true;
        }

        /// <summary>
        /// Request to close a document. The document will be closed
        /// (if possible) before returning.
        /// </summary>
        /// <param name="document">The document to close</param>
        /// <returns>True if the document was closed</returns>
        public async Task<bool> RequestCloseDocument(IDocument document)
        {
            bool canClose = await document.RequestClose();

            DocumentCloseMessage msg = new DocumentCloseMessage(document);
            await Oy.Publish("Document:RequestClose", msg);
            if (msg.Cancelled) canClose = false;

            if (canClose) ForceCloseDocument(document);
            return canClose;
        }

        public async Task ForceCloseDocument(IDocument document)
        {
            await Oy.Publish("Document:BeforeClose", document);

            _openDocuments.Remove(document);

            await Oy.Publish("Document:Closed", document);
        }

        private async Task OpenDocument(IDocument doc)
        {
            _openDocuments.Add(doc);
            await Oy.Publish("Document:Opened", doc);
            await ActivateDocument(doc);
        }
        
        // Settings provider

        public string Name => "CBRE.Shell.Documents";

        public IEnumerable<SettingKey> GetKeys()
        {
            yield return new SettingKey("FileAssociations", "Associations", typeof(FileAssociations));
        }

        public void LoadValues(ISettingsStore store)
        {
            if (!store.Contains("Associations")) return;

            FileAssociations associations = store.Get("Associations", new FileAssociations());
            AssociateExtensionHandlers(associations.Where(x => x.Value).Select(x => x.Key));
        }

        public void StoreValues(ISettingsStore store)
        {
            FileAssociations associations = new FileAssociations();
            List<string> reg = GetRegisteredExtensionAssociations().ToList();
            foreach (string ext in _loaders.SelectMany(x => x.SupportedFileExtensions).SelectMany(x => x.Extensions))
            {
                associations[ext] = reg.Contains(ext, StringComparer.InvariantCultureIgnoreCase);
            }
            store.Set("Associations", associations);
        }

        public class FileAssociations : Dictionary<string, bool>
        {
            public FileAssociations Clone()
            {
                FileAssociations b = new FileAssociations();
                foreach (KeyValuePair<string, bool> kv in this) b.Add(kv.Key, kv.Value);
                return b;
            }
        }

        private static string ExecutableLocation()
        {
            return Assembly.GetEntryAssembly().Location;
        }

        private void RegisterExtensionHandlers()
        {
            try
            {
                using (RegistryKey root = Registry.CurrentUser.OpenSubKey("Software\\Classes", true))
                {
                    if (root == null) return;

                    foreach (FileExtensionInfo ext in _loaders.SelectMany(x => x.SupportedFileExtensions))
                    {
                        foreach (string extension in ext.Extensions)
                        {
                            using (RegistryKey progId = root.CreateSubKey(_programId + extension + "." + _programIdVer))
                            {
                                if (progId == null) continue;

                                progId.SetValue("", ext.Description);

                                using (RegistryKey di = progId.CreateSubKey("DefaultIcon"))
                                {
                                    di?.SetValue("", ExecutableLocation() + ",-40001");
                                }

                                using (RegistryKey comm = progId.CreateSubKey("shell\\open\\command"))
                                {
                                    comm?.SetValue("", "\"" + ExecutableLocation() + "\" \"%1\"");
                                }

                                progId.SetValue("AppUserModelID", _programId);
                            }
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                // security exception or some such
            }
        }

        private void AssociateExtensionHandlers(IEnumerable<string> extensions)
        {
            try
            {
                using (RegistryKey root = Registry.CurrentUser.OpenSubKey("Software\\Classes", true))
                {
                    if (root == null) return;

                    foreach (string extension in extensions)
                    {
                        using (RegistryKey ext = root.CreateSubKey(extension))
                        {
                            if (ext == null) return;
                            ext.SetValue("", _programId + extension + "." + _programIdVer);
                            ext.SetValue("PerceivedType", "Document");

                            using (RegistryKey openWith = ext.CreateSubKey("OpenWithProgIds"))
                            {
                                openWith?.SetValue(_programId + extension + "." + _programIdVer, string.Empty);
                            }
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                // security exception or some such
            }
        }

        private IEnumerable<string> GetRegisteredExtensionAssociations()
        {
            List<string> associations = new List<string>();
            try
            {
                using (RegistryKey root = Registry.CurrentUser.OpenSubKey("Software\\Classes"))
                {
                    if (root == null) return Enumerable.Empty<string>();

                    foreach (FileExtensionInfo ft in _loaders.SelectMany(x => x.SupportedFileExtensions))
                    {
                        foreach (string extension in ft.Extensions)
                        {
                            using (RegistryKey ext = root.OpenSubKey(extension))
                            {
                                if (ext == null) continue;
                                if (Convert.ToString(ext.GetValue("")) == _programId + extension + "." + _programIdVer)
                                {
                                    associations.Add(extension);
                                }
                            }
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                // security exception or some such
            }

            return associations;
        }
    }
}