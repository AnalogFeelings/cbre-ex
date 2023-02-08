using System.Collections.Generic;
using System.Threading.Tasks;
using CBRE.FileSystem;

namespace CBRE.Providers.Texture
{
    public interface ITexturePackageProvider
    {
        Task<TexturePackage> GetTexturePackage(string name, TexturePackageReference reference);

        Task<IEnumerable<TexturePackage>> GetTexturePackages(string name, IEnumerable<TexturePackageReference> references);

        IEnumerable<TexturePackageReference> GetPackagesInFile(string name, IFile file);
    }
}