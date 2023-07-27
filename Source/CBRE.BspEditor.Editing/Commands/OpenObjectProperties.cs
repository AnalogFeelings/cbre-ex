using System.ComponentModel.Composition;
using System.Threading.Tasks;
using LogicAndTrick.Oy;
using CBRE.BspEditor.Commands;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Editing.Properties;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Hotkeys;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.Commands
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [MenuItem("Edit", "", "Properties", "B")]
    [CommandID("BspEditor:Map:Properties")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_ObjectProperties))]
    [DefaultHotkey("Alt+Enter")]
    public class OpenObjectProperties : BaseCommand
    {
        public override string Name { get; set; } = "Properties";
        public override string Details { get; set; } = "Open the object properties window.";

        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            await Oy.Publish("Context:Add", new ContextInfo("BspEditor:ObjectProperties"));
        }
    }
}