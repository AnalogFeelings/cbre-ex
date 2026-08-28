using CBRE.Common;
using CBRE.FileSystem;
using CBRE.Graphics.Helpers;
using CBRE.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CBRE.Providers.Model
{
    public abstract class ModelProvider
    {
        private static readonly List<ModelProvider> RegisteredProviders;
        private static readonly List<ModelReference> References;
        private static readonly Dictionary<string, DataStructures.Models.Model> Models;

        static ModelProvider()
        {
            RegisteredProviders = new List<ModelProvider>();
            References = new List<ModelReference>();
            Models = new Dictionary<string, DataStructures.Models.Model>();
        }

        public static void Register(ModelProvider provider)
        {
            RegisteredProviders.Add(provider);
        }

        public static void Deregister(ModelProvider provider)
        {
            RegisteredProviders.Remove(provider);
        }

        public static void DeregisterAll()
        {
            RegisteredProviders.Clear();
        }

        public static ModelReference CreateModelReference(IFile file)
        {
            DataStructures.Models.Model model = LoadModel(file);
            ModelReference reference = new ModelReference(file.FullPathName, model);
            References.Add(reference);
            return reference;
        }

        public static void DeleteModelReference(ModelReference reference)
        {
            References.Remove(reference);
            if (References.All(x => x.Model != reference.Model))
            {
                UnloadModel(reference.Model);
            }
        }

        public static bool CanLoad(IFile file)
        {
            return RegisteredProviders.Any(p => p.IsValidForFile(file));
        }

        private static DataStructures.Models.Model LoadModel(IFile file)
        {
            string path = file.FullPathName;
            if (Models.ContainsKey(path)) return Models[path];

            if (!file.Exists) throw new ProviderException(Local.LocalString("exception.supplied_file_not_exist"));
            ModelProvider provider = RegisteredProviders.FirstOrDefault(p => p.IsValidForFile(file));
            if (provider != null)
            {
                DataStructures.Models.Model model = provider.LoadFromFile(file);
                model.PreprocessModel();
                for (int i = 0; i < model.Textures.Count; i++)
                {
                    DataStructures.Models.Texture t = model.Textures[i];
                    t.TextureObject = TextureHelper.Create(Local.LocalString("data.model.provider", path, t.Name, i), t.Image, t.Image.Width, t.Image.Height, TextureFlags.None);
                }
                Models[path] = model;
                return model;
            }
            throw new ProviderNotFoundException(Local.LocalString("exception.no_model_provider_for_file"));
        }

        private static void UnloadModel(DataStructures.Models.Model model)
        {
            model.Dispose();
            List<KeyValuePair<string, DataStructures.Models.Model>> kvs = Models.Where(x => x.Value == model).ToList();
            foreach (KeyValuePair<string, DataStructures.Models.Model> kv in kvs) Models.Remove(kv.Key);
        }

        protected abstract bool IsValidForFile(IFile file);
        protected abstract DataStructures.Models.Model LoadFromFile(IFile file);
    }
}
