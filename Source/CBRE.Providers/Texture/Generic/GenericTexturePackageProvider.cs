using CBRE.Common.Logging;
using CBRE.FileSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace CBRE.Providers.Texture.Generic
{
    [Export("Generic", typeof(ITexturePackageProvider))]
    public class GenericTexturePackageProvider : ITexturePackageProvider
    {
        public IEnumerable<TexturePackageReference> GetPackagesInFile(string name, IFile file)
        {
            yield return new TexturePackageReference(name, file);
        }

        public async Task<TexturePackage> GetTexturePackage(string name, TexturePackageReference reference)
        {
            return await Task.Factory.StartNew(() =>
            {
                return new GenericTexturePackage(name, reference);
            });
        }

        public async Task<IEnumerable<TexturePackage>> GetTexturePackages(string name, IEnumerable<TexturePackageReference> references)
        {
            return await Task.Factory.StartNew(() =>
            {
                return references.AsParallel().Select(reference =>
                {
                    try
                    {
                        return new GenericTexturePackage(name, reference);
                    }
                    catch (Exception ex)
                    {
                        Log.Debug(nameof(GenericTexturePackageProvider), $"Invalid generic file: {reference.File.Name} - {ex.Message}");

                        return null;
                    }
                }).Where(x => x != null);
            });
        }
    }
}
