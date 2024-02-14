using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Actions;
using CBRE.Editor.Actions.MapObjects.Entities;
using CBRE.Localization;
using System.Collections.Generic;
using System.Linq;

namespace CBRE.Editor.Problems
{
    public class DuplicateKeyValues : IProblemCheck
    {
        public IEnumerable<Problem> Check(Map map, bool visibleOnly)
        {
            List<Entity> entities = map.WorldSpawn.Find(x => x is Entity && (!visibleOnly || (!x.IsVisgroupHidden && !x.IsCodeHidden))).OfType<Entity>().ToList();
            foreach (Entity entity in entities)
            {
                IEnumerable<IGrouping<string, Property>> dupes = from p in entity.EntityData.Properties
                                                                 group p by p.Key.ToLowerInvariant()
                            into g
                                                                 where g.Count() > 1
                                                                 select g;
                if (dupes.Any())
                {
                    yield return new Problem(GetType(), map, new[] { entity }, Fix, Local.LocalString("document.duplicate_key"), Local.LocalString("document.duplicate_key.description"));
                }
            }
        }

        public IAction Fix(Problem problem)
        {
            EditEntityData edit = new EditEntityData();
            foreach (MapObject mo in problem.Objects)
            {
                EntityData ed = mo.GetEntityData().Clone();
                IEnumerable<IGrouping<string, Property>> dupes = from p in ed.Properties
                                                                 group p by p.Key.ToLowerInvariant()
                            into g
                                                                 where g.Count() > 1
                                                                 select g;
                foreach (Property prop in dupes.SelectMany(dupe => dupe.Skip(1)))
                {
                    ed.Properties.Remove(prop);
                }
                edit.AddEntity(mo, ed);
            }
            return edit;
        }
    }
}