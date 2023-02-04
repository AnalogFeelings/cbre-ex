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
using CBRE.DataStructures.GameData;

namespace CBRE.BspEditor.Editing.Problems
{
    [Export(typeof(IProblemCheck))]
    [AutoTranslate]
    public class InvalidKeyValues : IProblemCheck
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public Uri Url => null;
        public bool CanFix => true;

        public async Task<List<Problem>> Check(MapDocument document, Predicate<IMapObject> filter)
        {
            var gamedata = await document.Environment.GetGameData();
            return document.Map.Root.FindAll()
                .Where(x => filter(x))
                .Select(x => new { Object = x, EntityData = x.Data.GetOne<EntityData>() })
                .Where(x => x.EntityData != null && GetInvalidKeys(gamedata, x.EntityData).Any())
                .Select(x => new Problem().Add(x.Object))
                .ToList();
        }

        private IEnumerable<string> GetInvalidKeys(GameData gamedata, EntityData data)
        {
            // Multimanagers require invalid key/values to work, exclude them from the search
            if (string.Equals(data.Name, "multi_manager", StringComparison.CurrentCultureIgnoreCase)) return new string[0];

            var cls = gamedata.GetClass(data.Name);
            if (cls == null) return new string[0];

            return data.Properties.Select(x => x.Key)
                .Except(cls.Properties.Select(x => x.Name), StringComparer.CurrentCultureIgnoreCase);
        }

        public async Task Fix(MapDocument document, Problem problem)
        {
            var gamedata = await document.Environment.GetGameData();

            var transaction = new Transaction();

            foreach (var obj in problem.Objects)
            {
                var data = obj.Data.GetOne<EntityData>();
                if (data == null) continue;

                var vals = new Dictionary<string, string>();

                foreach (var key in GetInvalidKeys(gamedata, data))
                {
                    vals[key] = null;
                }

                transaction.Add(new EditEntityDataProperties(obj.ID, vals));
            }

            await MapDocumentOperation.Perform(document, transaction);
        }
    }
}