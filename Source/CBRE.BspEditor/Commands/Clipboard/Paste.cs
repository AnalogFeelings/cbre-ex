using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using CBRE.BspEditor.Components;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations.Selection;
using CBRE.BspEditor.Modification.Operations.Tree;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.BspEditor.Properties;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Hotkeys;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Commands.Clipboard
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [CommandID("BspEditor:Edit:Paste")]
    [DefaultHotkey("Ctrl+V")]
    [MenuItem("Edit", "", "Clipboard", "F")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_Paste))]
    public class Paste : BaseCommand
    {
        private readonly Lazy<ClipboardManager> _clipboard;
        private readonly Random _random;

        public override string Name { get; set; } = "Paste";
        public override string Details { get; set; } = "Paste the current clipboard contents";

        [ImportingConstructor]
        public Paste([Import] Lazy<ClipboardManager> clipboard)
        {
            _clipboard = clipboard;
            _random = new Random();
        }

        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            if (_clipboard.Value.CanPaste())
            {
                // Work out a random offset to offset duplicate ids
                var step = Vector3.One * 16;

                // If there's a grid, use the grid spacing instead of the box dimensions
                var grid = document.Map.Data.GetOne<GridData>();
                if (grid?.Grid != null && grid.Grid.Spacing > 1)
                {
                    step = grid.Grid.AddStep(Vector3.Zero, Vector3.One);
                }

                // Get the pasted values, moving objects that have an id already in the map
                var content = _clipboard.Value.GetPastedContent(document, (d, o) => CopyAndMove(d, o, step)).ToList();

                var transaction = new Transaction(
                    new Deselect(document.Selection),
                    new Attach(document.Map.Root.ID, content),
                    new Select(content)
                );

                await MapDocumentOperation.Perform(document, transaction);
            }
        }

        private IMapObject CopyAndMove(MapDocument document, IMapObject o, Vector3 step)
        {
            var copy = (IMapObject)o.Copy(document.Map.NumberGenerator);
            copy.Transform(Matrix4x4.CreateTranslation(_random.Next(-4, 5) * step.X, _random.Next(-4, 5) * step.Y, _random.Next(-4, 5) * step.Z));
            return copy;
        }
    }
}