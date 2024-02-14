using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Actions;
using CBRE.Editor.Actions.MapObjects.Operations;
using CBRE.Localization;
using System.Collections.Generic;
using System.Linq;

namespace CBRE.Editor.Problems
{
    public class DuplicateFaceIDs : IProblemCheck
    {
        public IEnumerable<Problem> Check(Map map, bool visibleOnly)
        {
            IEnumerable<IGrouping<long, Face>> dupes = from o in map.WorldSpawn.Find(x => x is Solid && (!visibleOnly || (!x.IsVisgroupHidden && !x.IsCodeHidden)))
                            .OfType<Solid>()
                            .SelectMany(x => x.Faces)
                                                       group o by o.ID
                        into g
                                                       where g.Count() > 1
                                                       select g;
            foreach (IGrouping<long, Face> dupe in dupes)
            {
                yield return new Problem(GetType(), map, dupe, Fix, Local.LocalString("document.duplicate_id"), Local.LocalString("document.duplicate_id.description"));
            }
        }

        public IAction Fix(Problem problem)
        {
            return new EditFace(problem.Faces, (d, x) => x.ID = d.Map.IDGenerator.GetNextFaceID(), true);
        }
    }
}