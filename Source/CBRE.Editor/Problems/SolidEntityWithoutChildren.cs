using CBRE.DataStructures.GameData;
using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Actions;
using CBRE.Editor.Actions.MapObjects.Operations;
using CBRE.Localization;
using System.Collections.Generic;
using System.Linq;

namespace CBRE.Editor.Problems
{
    public class SolidEntityWithoutChildren : IProblemCheck
    {
        public IEnumerable<Problem> Check(Map map, bool visibleOnly)
        {
            foreach (Entity entity in map.WorldSpawn
                .Find(x => x is Entity && (!visibleOnly || (!x.IsVisgroupHidden && !x.IsCodeHidden)))
                .OfType<Entity>()
                .Where(x => x.GameData != null)
                .Where(x => x.GameData.ClassType == ClassType.Solid)
                .Where(x => !x.GetChildren().SelectMany(y => y.FindAll()).Any(y => y is Solid)))
            {
                yield return new Problem(GetType(), map, new[] { entity }, Fix, Local.LocalString("document.brush_entity_no_solid"), Local.LocalString("document.brush_entity_no_solid.description"));
            }
        }

        public IAction Fix(Problem problem)
        {
            return new Delete(problem.Objects.Select(x => x.ID));
        }
    }
}