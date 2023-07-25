using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CBRE.BspEditor.Environment;
using CBRE.FileSystem;
using CBRE.Providers.Model;
using CBRE.Providers.Texture;
using CBRE.Rendering.Engine;
using CBRE.Rendering.Interfaces;
using CBRE.Rendering.Resources;

namespace CBRE.BspEditor.Rendering.Resources
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ResourceCollection
    {
        private readonly IEnumerable<Lazy<IModelProvider>> _modelProviders;
        private readonly Lazy<EngineInterface> _engine;

        private readonly ConcurrentDictionary<string, HashSet<string>> _textures;
        private readonly ConcurrentDictionary<string, HashSet<ModelResource>> _models;
        private readonly ConcurrentDictionary<string, List<IResource>> _resources;

        [ImportingConstructor]
        public ResourceCollection(
            [ImportMany] IEnumerable<Lazy<IModelProvider>> modelProviders,
            [Import] Lazy<EngineInterface> engine
        )
        {
            _modelProviders = modelProviders;
            _engine = engine;

            _textures = new ConcurrentDictionary<string, HashSet<string>>();
            _models = new ConcurrentDictionary<string, HashSet<ModelResource>>();
            _resources = new ConcurrentDictionary<string, List<IResource>>();
        }

        /// <summary>
        /// Get a model from the collection. If the model isn't loaded already, it will be.
        /// </summary>
        /// <param name="environment">The environment to load the model from</param>
        /// <param name="path">The path to the model</param>
        /// <returns>Completion task for the model</returns>
        public async Task<IModel> GetModel(IEnvironment environment, string path)
        {
            EnsureEnvironment(environment);
            HashSet<ModelResource> mlist = _models[environment.ID];
            List<IResource> rlist = _resources[environment.ID];

            // Check if the model has already been loaded
            ModelResource existing = mlist.FirstOrDefault(x => string.Equals(x.Name, path, StringComparison.InvariantCultureIgnoreCase));
            if (existing != null) return existing.Model;

            // Find the file
            IFile file = environment.Root.TraversePath(path);
            if (file == null || !file.Exists) return null;

            // Find a provider for the file
            Lazy<IModelProvider> provider = _modelProviders.FirstOrDefault(x => x.Value.CanLoadModel(file));
            if (provider == null) return null;

            // Try to load the model
            IModel res = await provider.Value.LoadModel(file);
            if (res == null) return null;

            // Set up the model and return
            _engine.Value.CreateResource(res);

            rlist.Add(res);
            mlist.Add(new ModelResource(path, res));

            return res;
        }

        /// <summary>
        /// Create a renderable for a model and initialise it
        /// </summary>
        /// <param name="environment">The environment to add the model resource to</param>
        /// <param name="model">The model</param>
        /// <returns>A model renderable</returns>
        public IModelRenderable CreateModelRenderable(IEnvironment environment, IModel model)
        {
            EnsureEnvironment(environment);
            List<IResource> rlist = _resources[environment.ID];

            Lazy<IModelProvider> provider = _modelProviders.FirstOrDefault(x => x.Value.IsProvider(model));
            IModelRenderable res = provider?.Value.CreateRenderable(model);
            if (res == null) return null;

            _engine.Value.CreateResource(res);
            rlist.Add(res);

            return res;
        }
        
        /// <summary>
        /// Destroy a model renderable and remove it from the resource collection.
        /// This will NOT destroy the associated model.
        /// </summary>
        /// <param name="environment">The environment of the renderable</param>
        /// <param name="renderable">The renderable to remove</param>
        public void DestroyModelRenderable(IEnvironment environment, IModelRenderable renderable)
        {
            EnsureEnvironment(environment);
            List<IResource> rlist = _resources[environment.ID];

            _engine.Value.DestroyResource(renderable);
            rlist.Remove(renderable);
        }

        /// <summary>
        /// Upload any resources that have been collected to the collection.
        /// </summary>
        /// <param name="environment">The environment to load from</param>
        /// <param name="collector">The collector that gathered the resources</param>
        /// <returns>Completion task</returns>
        public async Task Upload(IEnvironment environment, ResourceCollector collector)
        {
            if (environment?.ID == null) return;
            EnsureEnvironment(environment);

            List<IResource> rlist = _resources[environment.ID];
            HashSet<string> tlist = _textures[environment.ID];

            HashSet<string> textures = collector.Textures.Except(tlist, StringComparer.InvariantCultureIgnoreCase).ToHashSet();

            if (textures.Any())
            {
                TextureCollection tc = await environment.GetTextureCollection();
                IEnumerable<TextureItem> items = await tc.GetTextureItems(textures);
                using (ITextureStreamSource ss = tc.GetStreamSource())
                {
                    // ReSharper disable once AccessToDisposedClosure : We know this closure completes before `ss` is disposed due to Task.WaitAll
                    List<Task<IResource>> tasks = items.Select(x => Task.Run(() => UploadTexture(environment, x, ss))).ToList();
                    await Task.WhenAll(tasks);
                    rlist.AddRange(tasks.Select(x => x.Result));
                    tlist.UnionWith(textures);
                }
            }
        }

        private void EnsureEnvironment(IEnvironment environment)
        {
            if (!_textures.ContainsKey(environment.ID)) _textures.TryAdd(environment.ID, new HashSet<string>());
            if (!_models.ContainsKey(environment.ID)) _models.TryAdd(environment.ID, new HashSet<ModelResource>());
            if (!_resources.ContainsKey(environment.ID)) _resources.TryAdd(environment.ID, new List<IResource>());
        }

        private async Task<IResource> UploadTexture(IEnvironment environment, TextureItem item, ITextureStreamSource source)
        {
            using (Bitmap bitmap = await source.GetImage(item.Name, 512, 512))
            {
                BitmapData lb = bitmap.LockBits(new Rectangle(0, 0, item.Width, item.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] data = new byte[lb.Stride * lb.Height];
                Marshal.Copy(lb.Scan0, data, 0, data.Length);
                bitmap.UnlockBits(lb);
                
                return _engine.Value.UploadTexture($"{environment.ID}::{item.Name}", bitmap.Width, bitmap.Height, data, TextureSampleType.Standard);
            }
        }

        /// <summary>
        /// Dispose all resources for environments not in the given list.
        /// </summary>
        /// <param name="usedEnvironments">The environments to retain resources for</param>
        public void DisposeOtherEnvironments(HashSet<IEnvironment> usedEnvironments)
        {
            foreach (string dt in _textures.Keys.Except(usedEnvironments.Select(x => x.ID)).ToList())
            {
                _textures.TryRemove(dt, out _);
            }
            foreach (string dm in _models.Keys.Except(usedEnvironments.Select(x => x.ID)).ToList())
            {
                _models.TryRemove(dm, out _);
            }
            foreach (string dr in _resources.Keys.Except(usedEnvironments.Select(x => x.ID)).ToList())
            {
                List<IResource> list = _resources[dr];
                _resources.TryRemove(dr, out _);
                foreach (IResource res in list) _engine.Value.DestroyResource(res);
            }
        }

        private class ModelResource
        {
            public string Name { get; }
            public IModel Model { get; }

            public ModelResource(string name, IModel model)
            {
                Name = name;
                Model = model;
            }
        }
    }
}