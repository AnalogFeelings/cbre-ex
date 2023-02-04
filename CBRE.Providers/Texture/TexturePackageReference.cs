using CBRE.FileSystem;

namespace CBRE.Providers.Texture
{
    /// <summary>
    /// A lightweight reference to a texture package
    /// </summary>
    public class TexturePackageReference
    {
        public string Name { get; private set; }
        public IFile File { get; private set; }

        public TexturePackageReference(string name, IFile file)
        {
            Name = name;
            File = file;
        }
    }
}