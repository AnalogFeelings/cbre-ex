using System.ComponentModel.Composition;
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
    [CommandID("BspEditor:Edit:SelectNone")]
    [DefaultHotkey("Shift+Q")]
    [MenuItem("Edit", "", "Selection", "D")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_SelectNone))]
    public class SelectNone : BaseCommand
    {
        public override string Name { get; set; } = "Select None";
        public override string Details { get; set; } = "Clear selection";

        protected override Task Invoke(MapDocument document, CommandParameters parameters)
        {
            Deselect op = new Deselect(document.Map.Root.FindAll());
            return MapDocumentOperation.Perform(document, op);
        }
    }
}