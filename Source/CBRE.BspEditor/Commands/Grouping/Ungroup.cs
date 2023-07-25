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

namespace CBRE.BspEditor.Commands.Grouping
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [CommandID("BspEditor:Edit:Ungroup")]
    [DefaultHotkey("Ctrl+U")]
    [MenuItem("Tools", "", "Group", "D")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_Ungroup))]
    public class Ungroup : BaseCommand
    {
        public override string Name { get; set; } = "Ungroup";
        public override string Details { get; set; } = "Ungroup selected objects";

        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            System.Collections.Generic.List<Primitives.MapObjects.Group> sel = document.Selection.GetSelectedParents().OfType<Primitives.MapObjects.Group>().ToList();
            if (sel.Count > 0)
            {
                Transaction tns = new Transaction();
                foreach (Primitives.MapObjects.Group grp in sel)
                {
                    System.Collections.Generic.List<Primitives.MapObjects.IMapObject> list = grp.Hierarchy.ToList();
                    tns.Add(new Detatch(grp.ID, list));
                    tns.Add(new Attach(document.Map.Root.ID, list));
                    tns.Add(new Detatch(grp.Hierarchy.Parent.ID, grp));
                }
                await MapDocumentOperation.Perform(document, tns);
            }
        }
    }
}