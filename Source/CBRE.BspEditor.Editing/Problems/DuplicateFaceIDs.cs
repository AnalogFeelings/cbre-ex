using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations.Data;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.Problems
{
    [Export(typeof(IProblemCheck))]
    [AutoTranslate]
    public class DuplicateFaceIDs : IProblemCheck
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public Uri Url => null;
        public bool CanFix => true;

        public Task<List<Problem>> Check(MapDocument document, Predicate<IMapObject> filter)
        {
            List<Problem> dupes = document.Map.Root.FindAll()
                .Where(x => filter(x))
                .SelectMany(x => x.Data.OfType<Face>().Select(f => new { Object = x, Face = f }))
                .GroupBy(x => x.Face.ID)
                .Where(x => x.Count() > 1)
                .Select(x => new Problem().Add(x.Select(o => o.Object)).Add(x.Select(o => o.Face)))
                .ToList();

            return Task.FromResult(dupes);
        }

        public Task Fix(MapDocument document, Problem problem)
        {
            Transaction edit = new Transaction();

            foreach (IMapObject obj in problem.Objects)
            {
                foreach (Face face in obj.Data.Intersect(problem.ObjectData).OfType<Face>())
                {
                    Face copy = (Face) face.Copy(document.Map.NumberGenerator);

                    edit.Add(new RemoveMapObjectData(obj.ID, face));
                    edit.Add(new AddMapObjectData(obj.ID, copy));
                }
            }

            return MapDocumentOperation.Perform(document, edit);
        }
    }
}