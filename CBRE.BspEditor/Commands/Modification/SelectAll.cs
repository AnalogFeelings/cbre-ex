using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations.Selection;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.BspEditor.Properties;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Hotkeys;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Commands.Modification
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [CommandID("BspEditor:Edit:SelectAll")]
    [DefaultHotkey("Ctrl+A")]
    [MenuItem("Edit", "", "Selection", "B")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_SelectAll))]
    public class SelectAll : BaseCommand
    {
        public override string Name { get; set; } = "Select All";
        public override string Details { get; set; } = "Select all objects";

        protected override Task Invoke(MapDocument document, CommandParameters parameters)
        {
            var op = new Select(document.Map.Root.FindAll().Where(x => x.Hierarchy.Parent != null));
            return MapDocumentOperation.Perform(document, op);
        }
    }
}
