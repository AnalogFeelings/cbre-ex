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
                //if (!reference.File.Exists || (!string.Equals(reference.File.Extension, "png", StringComparison.InvariantCultureIgnoreCase) &&
                //                               !string.Equals(reference.File.Extension, "jpg", StringComparison.InvariantCultureIgnoreCase) &&
                //                               !string.Equals(reference.File.Extension, "jpeg", StringComparison.InvariantCultureIgnoreCase)))
                //    return null;

                return new GenericTexturePackage(name, reference);
            });
        }

        public async Task<IEnumerable<TexturePackage>> GetTexturePackages(string name, IEnumerable<TexturePackageReference> references)
        {
            return await Task.Factory.StartNew(() =>
            {
                return references.AsParallel().Select(reference =>
                {
                    //if (!reference.File.Exists || (!string.Equals(reference.File.Extension, "png", StringComparison.InvariantCultureIgnoreCase) &&
                    //                               !string.Equals(reference.File.Extension, "jpg", StringComparison.InvariantCultureIgnoreCase) &&
                    //                               !string.Equals(reference.File.Extension, "jpeg", StringComparison.InvariantCultureIgnoreCase)))
                    //    return null;
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
