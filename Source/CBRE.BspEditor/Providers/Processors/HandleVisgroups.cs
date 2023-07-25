using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;

namespace CBRE.BspEditor.Providers.Processors
{
    /// <summary>
    /// Populates visgroup object lists and visibilities during map load
    /// </summary>
    [Export(typeof(IBspSourceProcessor))]
    public class HandleVisgroups : IBspSourceProcessor
    {
        public string OrderHint => "C";

        public Task AfterLoad(MapDocument document)
        {
            List<IMapObject> allObjects = document.Map.Root.FindAll();

            // add objects in each group to the visgroup list
            Dictionary<long, Visgroup> visgroups = document.Map.Data.Get<Visgroup>().ToDictionary(x => x.ID, x => x);
            foreach (IMapObject obj in allObjects)
            {
                List<VisgroupID> ids = obj.Data.Get<VisgroupID>().ToList();
                bool visible = true;
                foreach (VisgroupID id in ids)
                {
                    if (!visgroups.ContainsKey(id.ID)) continue;

                    Visgroup vis = visgroups[id.ID];
                    vis.Objects.Add(obj);
                    visible = vis.Visible;
                }
                
                // hide objects in hidden visgroups
                obj.Data.Replace(new VisgroupHidden(!visible));
            }

            // set up auto visgroups
            List<AutomaticVisgroup> autoVis = document.Environment?.GetAutomaticVisgroups()?.ToList() ?? new List<AutomaticVisgroup>();

            foreach (AutomaticVisgroup av in autoVis)
            {
                document.Map.Data.Add(av);
                foreach (IMapObject obj in allObjects)
                {
                    if (av.IsMatch(obj))
                    {
                        av.Objects.Add(obj);
                    }
                }
            }


            return Task.FromResult(0);
        }

        public Task BeforeSave(MapDocument document)
        {
            return Task.FromResult(0);
        }
    }
}