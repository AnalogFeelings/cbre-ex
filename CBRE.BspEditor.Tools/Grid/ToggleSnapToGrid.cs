using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Tools.Properties;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Hotkeys;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Tools.Grid
{
    [Export(typeof(ICommand))]
    [CommandID("BspEditor:Grid:ToggleSnapToGrid")]
    [DefaultHotkey("Shift+W")]
    [MenuItem("Map", "", "Grid", "B")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_SnapToGrid))]
    [AutoTranslate]
    public class ToggleSnapToGrid : ICommand
    {
        public string Name { get; set; } = "Snap to Grid";
        public string Details { get; set; } = "Toggle grid snapping";
        public bool IsInContext(IContext context)
        {
            return context.TryGet("ActiveDocument", out MapDocument _);
        }

        public async Task Invoke(IContext context, CommandParameters parameters)
        {
            if (context.TryGet("ActiveDocument", out MapDocument doc))
            {
                var activeGrid = doc.Map.Data.GetOne<GridData>();
                if (activeGrid != null)
                {
                    var operation = new TrivialOperation(x => activeGrid.SnapToGrid = !activeGrid.SnapToGrid, x => x.Update(activeGrid));
                    await MapDocumentOperation.Perform(doc, operation);
                }
            }
        }
    }
}