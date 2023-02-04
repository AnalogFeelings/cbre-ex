using System.Runtime.Serialization;
using CBRE.BspEditor.Primitives.MapObjects;

namespace CBRE.BspEditor.Primitives.MapData
{
    /// <summary>
    /// Base interface for generic map metadata
    /// </summary>
    public interface IMapData : ISerializable, IMapElement
    {
        bool AffectsRendering { get; }
    }
}