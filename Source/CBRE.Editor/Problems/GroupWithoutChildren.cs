using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Actions;
using CBRE.Editor.Actions.MapObjects.Operations;
using CBRE.Localization;
using System.Collections.Generic;
using System.Linq;

namespace CBRE.Editor.Problems
{
    public class GroupWithoutChildren : IProblemCheck
    {
        public IEnumerable<Problem> Check(Map map, bool visibleOnly)
        {
            foreach (Group group in map.WorldSpawn
                .Find(x => x is Group && (!visibleOnly || (!x.IsVisgroupHidden && !x.IsCodeHidden)))
                .OfType<Group>()
                .Where(x => !x.GetChildren().Any()))
            {
                yield return new Problem(GetType(), map, new[] { @group }, Fix, Local.LocalString("document.empty_group"), Local.LocalString("document.empty_group.description"));
            }
        }

        public IAction Fix(Problem problem)
        {
            return new Delete(problem.Objects.Select(x => x.ID));
        }
    }
}