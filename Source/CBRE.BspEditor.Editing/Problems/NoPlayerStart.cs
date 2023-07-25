using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations.Tree;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.Problems
{
    [Export(typeof(IProblemCheck))]
    [AutoTranslate]
    public class NoPlayerStart : IProblemCheck
    {
        public string Name { get; set; }
        public string Details { get; set; }

        public Uri Url => null;
        public bool CanFix => true;

        public Task<List<Problem>> Check(MapDocument document, Predicate<IMapObject> filter)
        {
            List<Problem> list = new List<Problem>();

            if (filter(document.Map.Root) && !document.Map.Root.Find(x => x is Entity && string.Equals(x.Data.GetOne<EntityData>()?.Name, "info_player_start", StringComparison.InvariantCultureIgnoreCase)).Any())
            {
                list.Add(new Problem());
            }

            return Task.FromResult(list);
        }

        public async Task Fix(MapDocument document, Problem problem)
        {
            Entity entity = new Entity(document.Map.NumberGenerator.Next("MapObject"))
            {
                Data =
                {
                    new EntityData { Name = "info_player_start" },
                    new ObjectColor(Colour.GetDefaultEntityColour()),
                    new Origin(Vector3.Zero),
                },
                IsSelected = false
            };

            Attach action = new Attach(document.Map.Root.ID, entity);
            await MapDocumentOperation.Perform(document, action);
        }
    }
}