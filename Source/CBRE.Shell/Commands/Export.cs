using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogicAndTrick.Oy;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Documents;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;
using CBRE.Shell.Properties;

namespace CBRE.Shell.Commands
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [CommandID("File:Export")]
    [MenuItem("File", "", "File", "L")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_Export))]
    public class Export : ICommand
    {
        private readonly IEnumerable<Lazy<IDocumentLoader>> _loaders;

        public string Name { get; set; } = "Export...";
        public string Details { get; set; } = "Export...";

        [ImportingConstructor]
        public Export([ImportMany] IEnumerable<Lazy<IDocumentLoader>> loaders)
        {
            _loaders = loaders;
        }

        public bool IsInContext(IContext context)
        {
            return context.TryGet("ActiveDocument", out IDocument _);
        }

        public async Task Invoke(IContext context, CommandParameters parameters)
        {
            IDocument doc = context.Get<IDocument>("ActiveDocument");
            if (doc != null)
            {
                List<IDocumentLoader> loaders = _loaders.Select(x => x.Value).Where(x => x.CanSave(doc)).ToList();

                List<string> filter = loaders.SelectMany(x => x.SupportedFileExtensions).Select(x => x.Description + "|" + String.Join(";", x.Extensions.Select(e => "*" + e))).ToList();

                using (SaveFileDialog sfd = new SaveFileDialog { Filter = String.Join("|", filter) })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        IDocumentLoader loader = loaders.FirstOrDefault(x => x.CanLoad(doc.FileName));
                        if (loader != null)
                        {
                            await Oy.Publish("Document:BeforeSave", doc);
                            await loader.Save(doc, sfd.FileName);
                        }
                    }
                }
            }
        }
    }
}