using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogicAndTrick.Oy;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Documents;
using CBRE.Common.Shell.Hotkeys;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;
using CBRE.Shell.Properties;
using CBRE.Shell.Registers;

namespace CBRE.Shell.Commands
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [CommandID("File:Save")]
    [DefaultHotkey("Ctrl+S")]
    [MenuItem("File", "", "File", "H")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_Save))]
    public class SaveFile : ICommand
    {
        private readonly Lazy<DocumentRegister> _documentRegister;

        public string Name { get; set; } = "Save";
        public string Details { get; set; } = "Save";

        [ImportingConstructor]
        public SaveFile(
            [Import] Lazy<DocumentRegister> documentRegister
        )
        {
            _documentRegister = documentRegister;
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
                string filename = doc.FileName;

                if (filename == null || !Directory.Exists(Path.GetDirectoryName(filename)))
                {
                    List<string> filter = _documentRegister.Value.GetSupportedFileExtensions(doc)
                        .Select(x => x.Description + "|" + string.Join(";", x.Extensions.Select(ex => "*" + ex)))
                        .ToList();

                    using (SaveFileDialog sfd = new SaveFileDialog {Filter = string.Join("|", filter)})
                    {
                        if (sfd.ShowDialog() != DialogResult.OK) return;
                        filename = sfd.FileName;
                    }
                }

                await _documentRegister.Value.SaveDocument(doc, filename);
            }
        }
    }
}