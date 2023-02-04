using System;
using System.ComponentModel.Composition;
using System.Runtime.Serialization;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Transport;

namespace CBRE.BspEditor.Primitives.MapData
{
    [Serializable]
    public class DisplayFlags : IMapData
    {
        public bool AffectsRendering => true;

        public bool HideNullTextures { get; set; } = false;
        public bool HideDisplacementSolids { get; set; } = false;

        public DisplayFlags()
        {
        }

        public DisplayFlags(SerialisedObject obj)
        {
            HideNullTextures = obj.Get<bool>("HideNullTextures");
            HideDisplacementSolids = obj.Get<bool>("HideDisplacementSolids");
        }

        [Export(typeof(IMapElementFormatter))]
        public class DisplayFlagsFormatter : StandardMapElementFormatter<DisplayFlags> { }
        
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("HideNullTextures", HideNullTextures);
            info.AddValue("HideDisplacementSolids", HideDisplacementSolids);
        }

        public SerialisedObject ToSerialisedObject()
        {
            var so = new SerialisedObject("DisplayFlags");
            so.Set("HideNullTextures", HideNullTextures);
            so.Set("HideDisplacementSolids", HideDisplacementSolids);
            return so;
        }

        public IMapElement Copy(UniqueNumberGenerator numberGenerator)
        {
            return Clone();
        }

        public IMapElement Clone()
        {
            return new DisplayFlags
            {
                HideNullTextures = HideNullTextures,
                HideDisplacementSolids = HideDisplacementSolids
            };
        }
    }
}
