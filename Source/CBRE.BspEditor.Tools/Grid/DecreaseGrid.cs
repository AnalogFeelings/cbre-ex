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
    [CommandID("BspEditor:Grid:DecreaseSpacing")]
    [DefaultHotkey("[")]
    [MenuItem("Map", "", "Grid", "G")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_SmallerGrid))]
    [AutoTranslate]
    public class DecreaseGrid : ICommand
    {
        public string Name { get; set; } = "Smaller Grid";
        public string Details { get; set; } = "Decrease the grid size";

        public bool IsInContext(IContext context)
        {
            return context.TryGet("ActiveDocument", out MapDocument _);
        }

        public async Task Invoke(IContext context, CommandParameters parameters)
        {
            if (context.TryGet("ActiveDocument", out MapDocument doc))
            {
                GridData activeGrid = doc.Map.Data.GetOne<GridData>();
                BspEditor.Grid.IGrid grid = activeGrid?.Grid;
                if (grid != null)
                {
                    TrivialOperation operation = new TrivialOperation(x => grid.Spacing--, x => x.Update(activeGrid));
                    await MapDocumentOperation.Perform(doc, operation);
                }
            }
        }
    }
}