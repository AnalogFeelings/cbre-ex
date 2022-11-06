using Assimp;
using System.IO;

namespace Assimp
{
    public static class BinaryExtensions
    {
        public static Vector3D ReadVector3D(this BinaryReader reader)
        {
            Vector3D retVal;
            retVal.X = reader.ReadSingle();
            retVal.Y = reader.ReadSingle();
            retVal.Z = reader.ReadSingle();
            return retVal;
        }

        public static Vector2D ReadVector2D(this BinaryReader reader)
        {
            Vector2D retVal;
            retVal.X = reader.ReadSingle();
            retVal.Y = reader.ReadSingle();
            return retVal;
        }
    }
}
