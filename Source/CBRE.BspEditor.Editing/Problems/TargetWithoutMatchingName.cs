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
    public class TargetWithoutMatchingName : IProblemCheck
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public Uri Url => null;
        public bool CanFix => true;

        public Task<List<Problem>> Check(MapDocument document, Predicate<IMapObject> filter)
        {
            // Unfiltered list of entities in the map
            var entities = document.Map.Root.FindAll()
                .OfType<Entity>()
                .Select(x => new { Object = x, x.EntityData })
                .Where(x => x.EntityData != null)
                .ToList();

            // Unfiltered list of targetnames in the map
            HashSet<string> targetnames = entities
                .Select(x => x.EntityData.Get("targetname", ""))
                .Where(x => x.Length > 0)
                .ToHashSet();

            // Filtered list of entities with targets without matching targetnames
            List<Problem> targets = entities
                .Where(x => filter(x.Object))
                .Select(x => new { x.Object, Target = x.EntityData.Get("target", "") })
                .Where(x => x.Target.Length > 0 && !targetnames.Contains(x.Target))
                .Select(x => new Problem { Text = x.Target }.Add(x.Object))
                .ToList();

            return Task.FromResult(targets);
        }

        public Task Fix(MapDocument document, Problem problem)
        {
            Transaction transaction = new Transaction();

            foreach (IMapObject obj in problem.Objects)
            {
                EntityData data = obj.Data.GetOne<EntityData>();
                if (data == null) continue;

                Dictionary<string, string> vals = new Dictionary<string, string> {["target"] = null};
                transaction.Add(new EditEntityDataProperties(obj.ID, vals));
            }

            return MapDocumentOperation.Perform(document, transaction);
        }
    }
}