using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CBRE.FileSystem;
using CBRE.Providers.Model.Mdl10.Format;
using CBRE.Rendering.Interfaces;

namespace CBRE.Providers.Model.Mdl10
{
    [Export(typeof(IModelProvider))]
    public class MdlModelProvider : IModelProvider
    {
        public bool CanLoadModel(IFile file)
        {
            return file.Exists && MdlFile.CanRead(file);
        }

        public async Task<IModel> LoadModel(IFile file)
        {
            return await Task.Factory.StartNew(() =>
            {
                var mdl = MdlFile.FromFile(file);
                mdl.WriteFakePrecalculatedChromeCoordinates();
                return new MdlModel(mdl);
            });
        }

        public bool IsProvider(IModel model)
        {
            return model is MdlModel;
        }

        public IModelRenderable CreateRenderable(IModel model)
        {
            return new MdlModelRenderable((MdlModel) model);
        }
    }
}