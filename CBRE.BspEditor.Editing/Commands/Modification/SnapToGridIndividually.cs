using System.ComponentModel.Composition;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using CBRE.BspEditor.Commands;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Editing.Properties;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations.Mutation;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.Commands.Modification
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [MenuItem("Tools", "", "Snap", "D")]
    [CommandID("BspEditor:Tools:SnapToGridIndividually")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_SnapSelectionIndividual))]
    public class SnapToGridIndividually : BaseCommand
    {
        public override string Name { get; set; } = "Snap to grid individually";
        public override string Details { get; set; } = "Snap selection to grid individually";

        protected override bool IsInContext(IContext context, MapDocument document)
        {
            return base.IsInContext(context, document) && !document.Selection.IsEmpty;
        }

        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            var grid = document.Map.Data.GetOne<GridData>();
            if (grid == null) return;
            
            var tl = document.Map.Data.GetOne<TransformationFlags>() ?? new TransformationFlags();

            var transaction = new Transaction();

            foreach (var mo in document.Selection.GetSelectedParents().ToList())
            {
                var box = mo.BoundingBox;

                var start = box.Start;
                var snapped = grid.Grid.Snap(start);
                var trans = snapped - start;
                if (trans == Vector3.Zero) continue;

                var tform = Matrix4x4.CreateTranslation(trans);

                var transformOperation = new BspEditor.Modification.Operations.Mutation.Transform(tform, mo);
                transaction.Add(transformOperation);

                // Check for texture transform
                if (tl.TextureLock) transaction.Add(new TransformTexturesUniform(tform, mo.FindAll()));
            }
            
            if (!transaction.IsEmpty)
            {
                await MapDocumentOperation.Perform(document, transaction);
            }
        }
    }
}