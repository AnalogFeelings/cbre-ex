using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using LogicAndTrick.Oy;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;

namespace CBRE.BspEditor.Modification.ChangeHandling
{
    /// <summary>
    /// Handles changes on visgroups and objects to maintain the object list and visibilities.
    /// </summary>
    [Export(typeof(IMapDocumentChangeHandler))]
    public class VisgroupHandler : IMapDocumentChangeHandler
    {
        public string OrderHint => "M";

        public async Task Changed(Change change)
        {
            bool changed = false;
            Dictionary<long, Visgroup> visgroups = change.Document.Map.Data.Get<Visgroup>().ToDictionary(x => x.ID, x => x);
            List<AutomaticVisgroup> autoVisgroups = change.Document.Map.Data.Get<AutomaticVisgroup>().ToList();

            // If an object is removed from a visgroup we should force it visible
            HashSet<IMapObject> makeVisible = new HashSet<IMapObject>();

            foreach (IMapObject mo in change.Added.Union(change.Updated))
            {
                HashSet<long> bef = new HashSet<long>(visgroups.Values.Where(x => x.Objects.Contains(mo)).Select(x => x.ID));
                HashSet<long> now = new HashSet<long>(mo.Data.OfType<VisgroupID>().Select(x => x.ID));

                // Remove visgroups the object was in before but isn't now
                foreach (long id in bef.Except(now))
                {
                    changed = true;
                    if (visgroups.ContainsKey(id)) visgroups[id].Objects.Remove(mo);
                    makeVisible.UnionWith(mo.FindAll());
                }

                // Add visgroups the object is in now but wasn't before
                foreach (long id in now.Except(bef))
                {
                    changed = true;
                    if (visgroups.ContainsKey(id)) visgroups[id].Objects.Add(mo);
                }

                // Handle autovisgroups as well
                foreach (AutomaticVisgroup av in autoVisgroups)
                {
                    bool match = av.IsMatch(mo);
                    bool contains = av.Objects.Contains(mo);

                    if (!match && contains)
                    {
                        av.Objects.Remove(mo);
                        changed = true;
                        makeVisible.UnionWith(mo.FindAll());
                    }
                    else if (match && !contains)
                    {
                        av.Objects.Add(mo);
                        changed = true;
                    }
                }
            }

            // Remove all deleted objects
            foreach (Visgroup vg in visgroups.Values)
            {
                int c = vg.Objects.Count;
                vg.Objects.ExceptWith(change.Removed);
                changed |= c != vg.Objects.Count;
            }

            foreach (AutomaticVisgroup av in autoVisgroups)
            {
                int c = av.Objects.Count;
                av.Objects.ExceptWith(change.Removed);
                changed |= c != av.Objects.Count;
            }

            if (makeVisible.Any())
            {
                foreach (IMapObject mv in makeVisible)
                {
                    mv.Data.Remove(x => x is VisgroupHidden);
                    change.Update(mv);
                }
            }

            // Fire event if changes were found
            if (changed)
            {
                await Oy.Publish("MapDocument:VisgroupsChanged", change.Document);
            }
        }
    }
}