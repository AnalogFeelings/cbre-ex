using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Actions;
using CBRE.Editor.Actions.MapObjects.Operations;
using CBRE.Extensions;
using CBRE.Localization;
using System.Collections.Generic;
using System.Linq;

namespace CBRE.Editor.Problems
{
    public class TextureAxisPerpendicularToFace : IProblemCheck
    {
        public IEnumerable<Problem> Check(Map map, bool visibleOnly)
        {
            List<Face> faces = map.WorldSpawn
                .Find(x => x is Solid && (!visibleOnly || (!x.IsVisgroupHidden && !x.IsCodeHidden)))
                .OfType<Solid>()
                .SelectMany(x => x.Faces)
                .ToList();
            foreach (Face face in faces)
            {
                DataStructures.Geometric.Coordinate normal = face.Texture.GetNormal();
                if (DMath.Abs(face.Plane.Normal.Dot(normal)) <= 0.0001m) yield return new Problem(GetType(), map, new[] { face }, Fix, Local.LocalString("document.texture_axis_face"), Local.LocalString("document.texture_axis_face.description"));
            }
        }

        public IAction Fix(Problem problem)
        {
            return new EditFace(problem.Faces, (d, x) => x.AlignTextureToFace(), false);
        }
    }
}