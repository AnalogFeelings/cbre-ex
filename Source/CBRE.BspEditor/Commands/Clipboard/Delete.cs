using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
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
    [CommandID("BspEditor:Edit:Delete")]
    [DefaultHotkey("Del")]
    [MenuItem("Edit", "", "Clipboard", "N")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_Delete))]
    public class Delete : BaseCommand
    {
        public override string Name { get; set; } = "Delete";
        public override string Details { get; set; } = "Delete the current selection";

        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            System.Collections.Generic.List<Primitives.MapObjects.IMapObject> sel = document.Selection.GetSelectedParents().ToList();
            if (sel.Any())
            {
                Transaction t = new Transaction(sel.GroupBy(x => x.Hierarchy.Parent.ID).Select(x => new Detatch(x.Key, x)));
                await MapDocumentOperation.Perform(document, t);
            }
        }
    }
}