using CBRE.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CBRE.DataStructures.MapObjects
{
    [Serializable]
    public class World : MapObject
    {
        public EntityData EntityData { get; set; }
        public List<Path> Paths { get; private set; }

        public World(long id) : base(id)
        {
            Paths = new List<Path>();
            EntityData = new EntityData { Name = Local.LocalString("data.world") };
        }

        protected World(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            EntityData = (EntityData)info.GetValue("EntityData", typeof(EntityData));
            Paths = ((Path[])info.GetValue("Paths", typeof(Path[]))).ToList();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("EntityData", EntityData);
            info.AddValue("Paths", Paths.ToArray());
        }

        public override MapObject Copy(IDGenerator generator)
        {
            World e = new World(generator.GetNextObjectID())
            {
                EntityData = EntityData.Clone(),
            };
            e.Paths.AddRange(Paths.Select(x => x.Clone()));
            CopyBase(e, generator);
            return e;
        }

        public override void Paste(MapObject o, IDGenerator generator)
        {
            PasteBase(o, generator);
            World e = o as World;
            if (e == null) return;
            EntityData = e.EntityData.Clone();
            Paths.Clear();
            Paths.AddRange(e.Paths.Select(x => x.Clone()));
        }

        public override MapObject Clone()
        {
            World e = new World(ID)
            {
                EntityData = EntityData.Clone(),
            };
            e.Paths.AddRange(Paths.Select(x => x.Clone()));
            CopyBase(e, null, true);
            return e;
        }

        public override void Unclone(MapObject o)
        {
            PasteBase(o, null, true);
            World e = o as World;
            if (e == null) return;
            EntityData = e.EntityData.Clone();
            Paths.Clear();
            Paths.AddRange(e.Paths.Select(x => x.Clone()));
        }

        public override EntityData GetEntityData()
        {
            return EntityData;
        }
    }
}
