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
    public class DuplicateKeyValues : IProblemCheck
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public Uri Url => null;
        public bool CanFix => true;

        public Task<List<Problem>> Check(MapDocument document, Predicate<IMapObject> filter)
        {
            List<Problem> entities = document.Map.Root.FindAll()
                .Where(x => filter(x))
                .Select(x => new { Object = x, EntityData = x.Data.GetOne<EntityData>() })
                .Where(x => x.EntityData != null && HasDuplicateKeyValues(x.EntityData))
                .Select(x => new Problem().Add(x.Object).Add(x.EntityData))
                .ToList();

            return Task.FromResult(entities);
        }

        private bool HasDuplicateKeyValues(EntityData data)
        {
            return data.Properties.GroupBy(x => x.Key.ToLowerInvariant()).Any(x => x.Count() > 1);
        }

        public Task Fix(MapDocument document, Problem problem)
        {
            // This error should only come from external sources (map loading, etc) as the EditEntityDataProperties
            // simply doesn't allow adding duplicate keys at all.

            Transaction transaction = new Transaction();

            foreach (IMapObject obj in problem.Objects)
            {
                EntityData data = obj.Data.GetOne<EntityData>();
                if (data == null) continue;

                Dictionary<string, string> vals = new Dictionary<string, string>();

                // Set the key to the first found value
                IEnumerable<IGrouping<string, KeyValuePair<string, string>>> groups = data.Properties.GroupBy(x => x.Key.ToLowerInvariant()).Where(x => x.Count() > 1);
                foreach (IGrouping<string, KeyValuePair<string, string>> g in groups)
                {
                    vals[g.Key] = g.First().Value;
                }

                transaction.Add(new EditEntityDataProperties(obj.ID, vals));
            }

            return MapDocumentOperation.Perform(document, transaction);
        }
    }
}