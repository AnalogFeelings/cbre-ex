using System.ComponentModel.Composition;
using System.Numerics;
using System.Threading.Tasks;
using CBRE.BspEditor.Commands;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations.Mutation;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.Common;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;

namespace CBRE.BspEditor.Editing.Commands.Modification
{
    [Export(typeof(ICommand))]
    [CommandID("BspEditor:Tools:Rotate")]
    public class RotateSelection : BaseCommand
    {
        public override string Name { get; set; } = "Rotate";
        public override string Details { get; set; } = "Rotate";

        protected override bool IsInContext(IContext context, MapDocument document)
        {
            return base.IsInContext(context, document) && !document.Selection.IsEmpty;
        }
        
        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            DataStructures.Geometric.Box selBox = document.Selection.GetSelectionBoundingBox();

            Vector3 axis = parameters.Get<Vector3>("Axis");
            float amount = parameters.Get<float>("Angle");
            float radians = (float) MathHelper.DegreesToRadians(amount);

            TransformationFlags tl = document.Map.Data.GetOne<TransformationFlags>() ?? new TransformationFlags();

            Transaction transaction = new Transaction();

            Matrix4x4 tform = Matrix4x4.CreateTranslation(-selBox.Center)
                        * Matrix4x4.CreateFromAxisAngle(axis, radians)
                        * Matrix4x4.CreateTranslation(selBox.Center);

            BspEditor.Modification.Operations.Mutation.Transform transformOperation = new BspEditor.Modification.Operations.Mutation.Transform(tform, document.Selection.GetSelectedParents());
            transaction.Add(transformOperation);

            // Check for texture transform
            if (tl.TextureLock) transaction.Add(new TransformTexturesUniform(tform, document.Selection));

            await MapDocumentOperation.Perform(document, transaction);
        }
    }
}
