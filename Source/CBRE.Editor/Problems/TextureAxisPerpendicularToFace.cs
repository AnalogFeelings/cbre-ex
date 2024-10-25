using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Actions;
using CBRE.Editor.Actions.MapObjects.Operations;
using CBRE.Extensions;
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
                if (DMath.Abs(face.Plane.Normal.Dot(normal)) <= 0.0001m) yield return new Problem(GetType(), map, new[] { face }, Fix, "Texture axis perpendicular to face", "The texture axis of this face is perpendicular to the face plane. This occurs when manipulating objects with texture lock off, as well as various other operations. Re-align the texture to the face to repair. Fixing the problem will reset the textures to the face plane.");
            }
        }

        public IAction Fix(Problem problem)
        {
            return new EditFace(problem.Faces, (d, x) => x.AlignTextureToFace(), false);
        }
    }
}