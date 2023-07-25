using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Primitives.MapObjects;

namespace CBRE.BspEditor.Modification.Operations.Selection
{
    public class Deselect : IOperation
    {
        private readonly List<long> _idsToDeselect;
        public bool Trivial => false;

        public Deselect(params IMapObject[] objectsToDeselect)
        {
            _idsToDeselect = objectsToDeselect.Where(x => x.IsSelected).Select(x => x.ID).ToList();
        }

        public Deselect(IEnumerable<IMapObject> objectsToDeselect)
        {
            _idsToDeselect = objectsToDeselect.Where(x => x.IsSelected).Select(x => x.ID).ToList();
        }

        public async Task<Change> Perform(MapDocument document)
        {
            Change ch = new Change(document);

            foreach (long id in _idsToDeselect)
            {
                IMapObject o = document.Map.Root.FindByID(id);
                if (o != null)
                {
                    o.IsSelected = false;
                    ch.Update(o);
                }
            }

            return ch;
        }

        public async Task<Change> Reverse(MapDocument document)
        {
            Change ch = new Change(document);

            foreach (long id in _idsToDeselect)
            {
                IMapObject o = document.Map.Root.FindByID(id);
                if (o != null)
                {
                    o.IsSelected = true;
                    ch.Update(o);
                }
            }

            return ch;
        }
    }
}