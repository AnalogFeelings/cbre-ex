using System.Runtime.Serialization;
using CBRE.BspEditor.Primitives.MapObjects;

namespace CBRE.BspEditor.Primitives.MapObjectData
{
    /// <summary>
    /// Base interface for generic map object metadata
    /// </summary>
    public interface IMapObjectData : ISerializable, IMapElement
    {

    }
}