using System.Collections.Generic;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Transport;

namespace CBRE.BspEditor.Modification.Operations.Data
{
    public class EditEntityDataProperties : IOperation
    {
        private readonly long _id;
        private readonly Dictionary<string, string> _valuesToSet;
        private SerialisedObject _beforeState;
        public bool Trivial => false;

        public EditEntityDataProperties(long id, Dictionary<string, string> valuesToSet)
        {
            _id = id;
            _valuesToSet = valuesToSet;
        }

        public async Task<Change> Perform(MapDocument document)
        {
            Change ch = new Change(document);

            IMapObject obj = document.Map.Root.FindByID(_id);
            EntityData data = obj?.Data.GetOne<EntityData>();
            if (data != null)
            {
                _beforeState = data.ToSerialisedObject();
                foreach (KeyValuePair<string, string> kv in _valuesToSet)
                {
                    if (kv.Value == null) data.Properties.Remove(kv.Key);
                    else data.Properties[kv.Key] = kv.Value;
                }
                ch.Update(obj);
            }

            return ch;
        }

        public async Task<Change> Reverse(MapDocument document)
        {
            Change ch = new Change(document);

            IMapObject obj = document.Map.Root.FindByID(_id);
            if (obj != null && _beforeState != null)
            {
                EntityData ed = new EntityData(_beforeState);
                obj.Data.Replace(ed);
                ch.Update(obj);
            }

            return ch;
        }
    }
}
