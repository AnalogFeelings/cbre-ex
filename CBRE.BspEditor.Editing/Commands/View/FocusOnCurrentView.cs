using System.ComponentModel.Composition;
using System.Threading.Tasks;
using LogicAndTrick.Oy;
using CBRE.BspEditor.Commands;
using CBRE.BspEditor.Documents;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Hotkeys;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.Commands.View
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [CommandID("BspEditor:View:FocusCurrent")]
    [MenuItem("View", "", "SplitView", "B")]
    [DefaultHotkey("Shift+Z")]
    public class FocusOnCurrentView : BaseCommand
    {
        public override string Name { get; set; } = "Focus on current view";
        public override string Details { get; set; } = "Maximise the current view in the viewport grid";

        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            await Oy.Publish("BspEditor:SplitView:FocusCurrent");
        }
    }
}