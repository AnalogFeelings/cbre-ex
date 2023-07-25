using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Grid;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Hotkeys;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Tools.Grid
{
    [Export(typeof(ICommand))]
    [CommandID("BspEditor:Grid:CycleGrid")]
    [DefaultHotkey("Shift+R")]
    [AutoTranslate]
    public class SwitchGrid : ICommand
    {
        [ImportMany] private IGridFactory[] _grids;

        public string Name => "Switch grids";
        public string Details => "Cycle through grid types";

        public bool IsInContext(IContext context)
        {
            return context.TryGet("ActiveDocument", out MapDocument _);
        }

        public async Task Invoke(IContext context, CommandParameters parameters)
        {
            if (context.TryGet("ActiveDocument", out MapDocument doc))
            {
                if (!_grids.Any()) return;

                IGrid current = doc.Map.Data.GetOne<GridData>()?.Grid;
                int idx = current == null ? -1 : Array.FindIndex(_grids, x => x.IsInstance(current));
                idx = (idx + 1) % _grids.Length;

                IGrid grid = await _grids[idx].Create(doc.Environment);

                GridData gd = new GridData(grid);
                TrivialOperation operation = new TrivialOperation(x => doc.Map.Data.Replace(gd), x => x.Update(gd));

                await MapDocumentOperation.Perform(doc, operation);
            }
        }
    }
}