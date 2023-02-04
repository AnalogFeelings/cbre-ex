using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using LogicAndTrick.Oy;
using CBRE.BspEditor.Commands;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Editing.Properties;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.Commands
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [MenuItem("Map", "", "Properties", "A")]
    [CommandID("BspEditor:Map:RootProperties")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_MapProperties))]
    public class OpenRootProperties : BaseCommand
    {
        public override string Name { get; set; } = "Map properties";
        public override string Details { get; set; } = "Open the map properties window";

        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            await Oy.Publish("BspEditor:ObjectProperties:OpenWithSelection", new List<IMapObject> {document.Map.Root});
        }
    }
}