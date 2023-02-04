using System.ComponentModel.Composition;
using System.Threading.Tasks;
using LogicAndTrick.Oy;
using CBRE.BspEditor.Commands;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Editing.Properties;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.Commands
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [MenuItem("Map", "", "Properties", "F")]
    [CommandID("BspEditor:Map:EntityReport")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_EntityReport))]
    public class OpenEntityReport : BaseCommand
    {
        public override string Name { get; set; } = "Entity report";
        public override string Details { get; set; } = "Open the entity report window";

        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            await Oy.Publish("Context:Add", new ContextInfo("BspEditor:EntityReport"));
        }
    }
}