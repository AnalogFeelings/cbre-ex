using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBRE.FileSystem;
using CBRE.Rendering.Interfaces;

namespace CBRE.Providers.Model
{
    public interface IModelProvider
    {
        bool CanLoadModel(IFile file);
        Task<IModel> LoadModel(IFile file);

        bool IsProvider(IModel model);
        IModelRenderable CreateRenderable(IModel model);
    }
}
