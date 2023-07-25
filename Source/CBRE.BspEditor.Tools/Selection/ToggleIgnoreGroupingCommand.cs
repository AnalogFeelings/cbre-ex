using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CBRE.BspEditor.Commands;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Tools.Properties;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Hotkeys;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Tools.Selection
{
    [Export(typeof(ICommand))]
    [CommandID("BspEditor:Map:ToggleIgnoreGrouping")]
    [DefaultHotkey("Ctrl+W")]
    [MenuItem("Map", "", "Grouping", "B")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_IgnoreGrouping))]
    [AutoTranslate]
    public class ToggleIgnoreGroupingCommand : BaseCommand
    {
        public override string Name { get; set; } = "Ignore grouping";
        public override string Details { get; set; } = "Toggle ignore grouping on and off";
        protected override Task Invoke(MapDocument document, CommandParameters parameters)
        {
            SelectionOptions opt = document.Map.Data.GetOne<SelectionOptions>() ?? new SelectionOptions();
            opt.IgnoreGrouping = !opt.IgnoreGrouping;
            MapDocumentOperation.Perform(document, new TrivialOperation(x => x.Map.Data.Replace(opt), x => x.Update(opt)));
            return Task.CompletedTask;
        }
    }
}