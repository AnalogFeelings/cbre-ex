using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations.Tree;
using CBRE.BspEditor.Primitives;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.Problems
{
    [Export(typeof(IProblemCheck))]
    [AutoTranslate]
    public class InvalidSolid : IProblemCheck
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public Uri Url => null;
        public bool CanFix => true;

        public Task<List<Problem>> Check(MapDocument document, Predicate<IMapObject> filter)
        {
            List<Problem> solids = document.Map.Root.FindAll()
                .Where(x => filter(x))
                .OfType<Solid>()
                .Where(x => !x.IsValid())
                .Select(x => new Problem().Add(x))
                .ToList();

            return Task.FromResult(solids);
        }

        public Task Fix(MapDocument document, Problem problem)
        {
            Transaction delete = new Transaction();
            foreach (IGrouping<long, IMapObject> g in problem.Objects.GroupBy(x => x.Hierarchy.Parent.ID))
            {
                delete.Add(new Detatch(g.Key, g));
            }
            return MapDocumentOperation.Perform(document, delete);
        }
    }
}