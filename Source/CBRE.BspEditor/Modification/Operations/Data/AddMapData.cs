using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Primitives.MapData;

namespace CBRE.BspEditor.Modification.Operations.Data
{
    public class AddMapData : IOperation
    {
        private List<IMapData> _dataToAdd;
        public bool Trivial { get; private set; }

        public AddMapData(params IMapData[] dataToAdd) : this(false, dataToAdd)
        {
        }

        public AddMapData(IEnumerable<IMapData> dataToAdd) : this(false, dataToAdd)
        {
        }

        public AddMapData(bool trivial, params IMapData[] dataToAdd) : this(trivial, dataToAdd.AsEnumerable())
        {
        }

        public AddMapData(bool trivial, IEnumerable<IMapData> dataToAdd)
        {
            Trivial = trivial;
            _dataToAdd = dataToAdd.ToList();
        }

        public async Task<Change> Perform(MapDocument document)
        {
            Change ch = new Change(document);
            
            foreach (IMapData d in _dataToAdd)
            {
                document.Map.Data.Add(d);
                ch.Update(d);
            }

            return ch;
        }

        public async Task<Change> Reverse(MapDocument document)
        {
            Change ch = new Change(document);

            foreach (IMapData d in _dataToAdd)
            {
                document.Map.Data.Remove(d);
                ch.Update(d);
            }

            return ch;
        }
    }
}