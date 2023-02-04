using System.ComponentModel.Composition;
using System.Threading.Tasks;
using LogicAndTrick.Oy;
using CBRE.BspEditor.Commands;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Editing.Properties;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.Commands.View
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [CommandID("BspEditor:View:AutosizeViews")]
    [MenuItem("View", "", "SplitView", "B")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_AutosizeViews))]
    public class AutosizeViews : BaseCommand
    {
        public override string Name { get; set; } = "Autosize views";
        public override string Details { get; set; } = "Automatically resize the split views to be the same size.";

        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            await Oy.Publish("BspEditor:SplitView:Autosize");
        }
    }
}