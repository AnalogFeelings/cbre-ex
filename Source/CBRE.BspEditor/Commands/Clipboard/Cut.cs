using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using CBRE.BspEditor.Components;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations.Tree;
using CBRE.BspEditor.Properties;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Hotkeys;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Commands.Clipboard
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [CommandID("BspEditor:Edit:Cut")]
    [DefaultHotkey("Ctrl+X")]
    [MenuItem("Edit", "", "Clipboard", "B")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_Cut))]
    public class Cut : BaseCommand
    {
        private readonly Lazy<ClipboardManager> _clipboard;

        public override string Name { get; set; } = "Cut";
        public override string Details { get; set; } = "Copy the current selection and remove it";

        [ImportingConstructor]
        public Cut([Import] Lazy<ClipboardManager> clipboard)
        {
            _clipboard = clipboard;
        }

        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            System.Collections.Generic.List<Primitives.MapObjects.IMapObject> sel = document.Selection.GetSelectedParents().ToList();
            if (sel.Any())
            {
                _clipboard.Value.Push(sel);
                Transaction t = new Transaction(sel.GroupBy(x => x.Hierarchy.Parent.ID).Select(x => new Detatch(x.Key, x)));
                await MapDocumentOperation.Perform(document, t);
            }
        }
    }
}