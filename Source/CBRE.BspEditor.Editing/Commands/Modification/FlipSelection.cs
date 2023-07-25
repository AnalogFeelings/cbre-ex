using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using CBRE.BspEditor.Commands;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations.Mutation;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.Commands.Modification
{
    public abstract class FlipSelection : BaseCommand
    {
        public override string Name { get; set; } = "Flip";
        public override string Details { get; set; } = "Flip";

        protected override bool IsInContext(IContext context, MapDocument document)
        {
            return base.IsInContext(context, document) && !document.Selection.IsEmpty;
        }

        protected abstract Vector3 GetScale();

        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            DataStructures.Geometric.Box selBox = document.Selection.GetSelectionBoundingBox();

            TransformationFlags tl = document.Map.Data.GetOne<TransformationFlags>() ?? new TransformationFlags();

            Transaction transaction = new Transaction();

            Matrix4x4 tform = Matrix4x4.CreateTranslation(-selBox.Center)
                        * Matrix4x4.CreateScale(GetScale())
                        * Matrix4x4.CreateTranslation(selBox.Center);

            BspEditor.Modification.Operations.Mutation.Transform transformOperation = new BspEditor.Modification.Operations.Mutation.Transform(tform, document.Selection.GetSelectedParents());
            transaction.Add(transformOperation);

            transaction.Add(new FlipFaces(document.Selection));

            // Check for texture transform
            if (tl.TextureLock) transaction.Add(new TransformTexturesUniform(tform, document.Selection));

            await MapDocumentOperation.Perform(document, transaction);
        }

        private class FlipFaces : IOperation
        {
            private readonly List<long> _idsToTransform;

            public bool Trivial => false;

            public FlipFaces(IEnumerable<IMapObject> objectsToTransform)
            {
                _idsToTransform = objectsToTransform.Select(x => x.ID).ToList();
            }

            public Task<Change> Perform(MapDocument document)
            {
                Change ch = new Change(document);

                List<IMapObject> objects = _idsToTransform.Select(x => document.Map.Root.FindByID(x)).Where(x => x != null).ToList();

                foreach (IMapObject o in objects)
                {
                    foreach (Face it in o.Data.OfType<Face>())
                    {
                        it.Vertices.Flip();
                        ch.Update(o);
                    }
                }

                return Task.FromResult(ch);
            }

            public Task<Change> Reverse(MapDocument document)
            {
                return Perform(document); // Reversing this operation means just performing it again
            }
        }
    }

    [AutoTranslate]
    [Export(typeof(ICommand))]
    [MenuItem("Tools", "Flip", "FlipAlign", "B")]
    [CommandID("BspEditor:Tools:FlipX")]
    public class FlipSelectionX : FlipSelection
    {
        protected override Vector3 GetScale()
        {
            return new Vector3(-1, 1, 1);
        }
    }

    [AutoTranslate]
    [Export(typeof(ICommand))]
    [MenuItem("Tools", "Flip", "FlipAlign", "D")]
    [CommandID("BspEditor:Tools:FlipY")]
    public class FlipSelectionY : FlipSelection
    {
        protected override Vector3 GetScale()
        {
            return new Vector3(1, -1, 1);
        }
    }

    [AutoTranslate]
    [Export(typeof(ICommand))]
    [MenuItem("Tools", "Flip", "FlipAlign", "F")]
    [CommandID("BspEditor:Tools:FlipZ")]
    public class FlipSelectionZ : FlipSelection
    {
        protected override Vector3 GetScale()
        {
            return new Vector3(1, 1, -1);
        }
    }
}
