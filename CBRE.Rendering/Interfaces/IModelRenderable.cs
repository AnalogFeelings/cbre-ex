using System.Numerics;
using CBRE.Rendering.Renderables;
using CBRE.Rendering.Resources;

namespace CBRE.Rendering.Interfaces
{
    public interface IModelRenderable : IRenderable, IUpdateable, IResource
    {
        IModel Model { get; }
        Vector3 Origin { get; set; }
        Vector3 Angles { get; set; }
        int Sequence { get; set; }

        Matrix4x4 GetModelTransformation();
        (Vector3, Vector3) GetBoundingBox();
    }
}