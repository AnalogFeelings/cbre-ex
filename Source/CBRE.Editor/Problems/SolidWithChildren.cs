using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Actions;
using CBRE.Editor.Actions.MapObjects.Operations;
using CBRE.Localization;
using System.Collections.Generic;
using System.Linq;

namespace CBRE.Editor.Problems
{
    public class SolidWithChildren : IProblemCheck
    {
        public IEnumerable<Problem> Check(Map map, bool visibleOnly)
        {
            foreach (Solid solid in map.WorldSpawn
                .Find(x => x is Solid && (!visibleOnly || (!x.IsVisgroupHidden && !x.IsCodeHidden)))
                .OfType<Solid>()
                .Where(x => x.HasChildren))
            {
                yield return new Problem(GetType(), map, new[] { solid }, Fix, Local.LocalString("document.solid_children"), Local.LocalString("document.solid_children.description"));
            }
        }

        public IAction Fix(Problem problem)
        {
            return new Reparent(problem.Objects[0].Parent.ID, problem.Objects[0].GetChildren());
        }
    }
}