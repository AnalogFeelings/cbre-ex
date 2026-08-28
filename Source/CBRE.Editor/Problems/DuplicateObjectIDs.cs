using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Actions;
using CBRE.Editor.Actions.MapObjects.Operations;
using CBRE.Localization;
using System.Collections.Generic;
using System.Linq;

namespace CBRE.Editor.Problems
{
    public class DuplicateObjectIDs : IProblemCheck
    {
        public IEnumerable<Problem> Check(Map map, bool visibleOnly)
        {
            IEnumerable<IGrouping<long, MapObject>> dupes = from o in map.WorldSpawn.Find(x => (!visibleOnly || (!x.IsVisgroupHidden && !x.IsCodeHidden)))
                                                            group o by o.ID
                        into g
                                                            where g.Count() > 1
                                                            select g;
            foreach (IGrouping<long, MapObject> dupe in dupes)
            {
                yield return new Problem(GetType(), map, dupe, Fix, Local.LocalString("document.duplicate_object"), Local.LocalString("document.duplicate_object.description"));
            }
        }

        public IAction Fix(Problem problem)
        {
            return new ReplaceObjects(problem.Objects, problem.Objects.Select(x =>
            {
                MapObject c = x.Clone();
                c.ID = problem.Map.IDGenerator.GetNextObjectID();
                return c;
            }));
        }
    }
}