using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations.Tree;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.Problems
{
    [Export(typeof(IProblemCheck))]
    [AutoTranslate]
    public class EntityWithEntityChildren : IProblemCheck
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public Uri Url => null;
        public bool CanFix => true;

        public Task<List<Problem>> Check(MapDocument document, Predicate<IMapObject> filter)
        {
            List<Problem> parents = document.Map.Root.FindAll()
                .OfType<Entity>()
                .Where(x => x.Hierarchy.HasChildren)
                .Where(x => filter(x))
                .Where(x => x.Find(f => !ReferenceEquals(f, x) && f is Entity).Any())
                .Select(x => new Problem().Add(x))
                .ToList();

            return Task.FromResult(parents);
        }

        public Task Fix(MapDocument document, Problem problem)
        {
            Transaction transaction = new Transaction();
            
            foreach (IMapObject obj in problem.Objects.SelectMany(x => x.Find(f => f is Entity)).Distinct())
            {
                transaction.Add(new Detatch(obj.Hierarchy.Parent.ID, obj));
                transaction.Add(new Attach(document.Map.Root.ID, obj));
            }

            return MapDocumentOperation.Perform(document, transaction);
        }
    }
}