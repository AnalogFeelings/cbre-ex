using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;

namespace CBRE.BspEditor.Modification.Operations.Data
{
    public class RemoveMapObjectData : IOperation
    {
        private long _id;
        private List<IMapObjectData> _dataToRemove;
        public bool Trivial => false;

        public RemoveMapObjectData(long id, params IMapObjectData[] dataToRemove)
        {
            _id = id;
            _dataToRemove = dataToRemove.ToList();
        }

        public RemoveMapObjectData(long id, IEnumerable<IMapObjectData> dataToRemove)
        {
            _id = id;
            _dataToRemove = dataToRemove.ToList();
        }

        public async Task<Change> Perform(MapDocument document)
        {
            Change ch = new Change(document);

            IMapObject obj = document.Map.Root.FindByID(_id);
            if (obj != null)
            {
                foreach (IMapObjectData d in _dataToRemove)
                {
                    obj.Data.Remove(d);
                }
                obj.DescendantsChanged();
                ch.Update(obj);
            }

            return ch;
        }

        public async Task<Change> Reverse(MapDocument document)
        {
            Change ch = new Change(document);

            IMapObject obj = document.Map.Root.FindByID(_id);
            if (obj != null)
            {
                foreach (IMapObjectData d in _dataToRemove)
                {
                    obj.Data.Add(d);
                }
                obj.DescendantsChanged();
                ch.Update(obj);
            }

            return ch;
        }
    }
}