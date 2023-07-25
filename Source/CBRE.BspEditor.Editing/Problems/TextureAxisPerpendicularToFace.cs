using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Primitives.MapObjects;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using CBRE.BspEditor.Modification.Operations.Data;
using CBRE.BspEditor.Primitives;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.Problems
{
    [Export(typeof(IProblemCheck))]
    [AutoTranslate]
    public class TextureAxisPerpendicularToFace : IProblemCheck
    {
        public string Name { get; set; }
        public string Details { get; set; }

        public Uri Url => new Uri("https://twhl.info/wiki/page/Error%3A_Texture_axis_perpendicular_to_face");
        public bool CanFix => true;

        public Task<List<Problem>> Check(MapDocument document, Predicate<IMapObject> filter)
        {
            List<Problem> list = new List<Problem>();

            IEnumerable<Solid> solids = document.Map.Root.Find(x => x is Solid).OfType<Solid>().Where(x => filter(x));
            foreach (Solid solid in solids)
            {
                List<Face> perps = (
                    from face in solid.Faces
                    let normal = face.Texture.GetNormal()
                    where Math.Abs(Vector3.Dot(face.Plane.Normal, normal)) <= 0.0001
                    select face
                ).ToList();
                if (perps.Any()) list.Add(new Problem().Add(solid).Add(perps));
            }

            return Task.FromResult(list);
        }

        public Task Fix(MapDocument document, Problem problem)
        {
            Transaction edit = new Transaction();

            IMapObject obj = problem.Objects[0];
            foreach (Face face in problem.ObjectData.OfType<Face>())
            {
                Face clone = (Face) face.Clone();
                clone.Texture.AlignToNormal(face.Plane.Normal);

                edit.Add(new RemoveMapObjectData(obj.ID, face));
                edit.Add(new AddMapObjectData(obj.ID, clone));
            }

            return MapDocumentOperation.Perform(document, edit);
        }
    }
}