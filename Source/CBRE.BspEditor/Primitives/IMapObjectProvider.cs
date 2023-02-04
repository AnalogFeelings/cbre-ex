using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Transport;

namespace CBRE.BspEditor.Primitives
{
    public interface IMapElementFormatter
    {
        bool IsSupported(IMapElement obj);
        SerialisedObject Serialise(IMapElement elem);

        bool IsSupported(SerialisedObject elem);
        IMapElement Deserialise(SerialisedObject obj);
    }
}
