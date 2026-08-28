using CBRE.DataStructures.Geometric;
using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Documents;
using CBRE.FileSystem;
using CBRE.Localization;
using CBRE.Providers.Model;
using CBRE.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CBRE.Editor.Extensions
{
    public static class ModelExtensions
    {
        private const string ModelMetaKey = "Model";
        private const string ModelNameMetaKey = "ModelName";
        private const string ModelBoundingBoxMetaKey = "BoundingBox";

        public static bool UpdateModels(this Map map, Document document)
        {
            if (CBRE.Settings.View.DisableModelRendering) return false;

            Dictionary<string, ModelReference> cache = document.GetMemory<Dictionary<string, ModelReference>>("ModelCache");
            if (cache == null)
            {
                cache = new Dictionary<string, ModelReference>();
                document.SetMemory("ModelCache", cache);
            }

            return UpdateModels(document, map.WorldSpawn, cache);
        }

        public static bool UpdateModels(this Map map, Document document, IEnumerable<MapObject> objects)
        {
            if (CBRE.Settings.View.DisableModelRendering) return false;

            Dictionary<string, ModelReference> cache = document.GetMemory<Dictionary<string, ModelReference>>("ModelCache");
            if (cache == null)
            {
                cache = new Dictionary<string, ModelReference>();
                document.SetMemory("ModelCache", cache);
            }

            bool updated = false;
            foreach (MapObject mo in objects) updated |= UpdateModels(document, mo, cache);
            return updated;
        }

        private static bool UpdateModels(Document document, MapObject mo, Dictionary<string, ModelReference> cache)
        {
            bool updatedChildren = false;
            foreach (MapObject child in mo.GetChildren()) updatedChildren |= UpdateModels(document, child, cache);

            Entity e = mo as Entity;
            if (e == null || !ShouldHaveModel(e))
            {
                bool has = e != null && HasModel(e);
                if (has) UnsetModel(e);
                return updatedChildren || has;
            }

            string model = GetModelName(e);
            string existingModel = e.MetaData.Get<string>(ModelNameMetaKey);
            if (String.Equals(model, existingModel, StringComparison.OrdinalIgnoreCase)) return updatedChildren; // Already set; No need to continue

            if (cache.ContainsKey(model))
            {
                ModelReference mr = cache[model];
                if (mr == null) UnsetModel(e);
                else SetModel(e, mr);
                return true;
            }
            else
            {
                string modelPath = Directories.GetModelPath(model);
                NativeFile file = null;
                if (!string.IsNullOrEmpty(modelPath))
                {
                    file = new NativeFile(modelPath);
                }
                if (file == null || !ModelProvider.CanLoad(file))
                {
                    // Model not valid, get rid of it
                    UnsetModel(e);
                    cache.Add(model, null);
                    return true;
                }

#if !DEBUG
                try
                {
#endif
                ModelReference mr = ModelProvider.CreateModelReference(file);
                SetModel(e, mr);
                cache.Add(model, mr);
                return true;
#if !DEBUG
                }
                catch (Exception exception)
                {
                    File.AppendAllText("modelLoadErrors.txt",  "\n" + Local.LocalString("error.failed_to_load", file.FullPathName) +
                        $"{exception.Message} ({exception.GetType().Name})\n" +
                        $"{exception.StackTrace}");
                    // Couldn't load
                    cache.Add(model, null);
                    return updatedChildren;
                }
#endif
            }
        }

        private static bool ShouldHaveModel(Entity entity)
        {
            return GetModelName(entity) != null;
        }

        private static string GetModelName(Entity entity)
        {
            if (entity.GameData == null) return null;
            
            bool usesModelRendering = entity.GameData.Behaviours.FirstOrDefault(x => x.Name == "useModels") != null;

            if (usesModelRendering)
            {
                return System.IO.Path.GetFileNameWithoutExtension(entity.EntityData.GetPropertyValue("file"));
            }

            return null;
        }

        private static void SetModel(Entity entity, ModelReference model)
        {
            entity.MetaData.Set(ModelMetaKey, model);
            entity.MetaData.Set(ModelNameMetaKey, GetModelName(entity));
            entity.MetaData.Set(ModelBoundingBoxMetaKey, model.Model.GetBoundingBox());
            entity.UpdateBoundingBox();
        }

        private static void UnsetModel(Entity entity)
        {
            entity.MetaData.Unset(ModelMetaKey);
            entity.MetaData.Unset(ModelNameMetaKey);
            entity.MetaData.Unset(ModelBoundingBoxMetaKey);
            entity.UpdateBoundingBox();
        }

        public static ModelReference GetModel(this Entity entity)
        {
            return entity.MetaData.Get<ModelReference>(ModelMetaKey);
        }

        public static bool HasModel(this Entity entity)
        {
            return entity.MetaData.Has<ModelReference>(ModelMetaKey);
        }

        public static int HideDistance(this Entity entity)
        {
            if (HasModel(entity))
            {
                int dist = CBRE.Settings.View.ModelRenderDistance;
                decimal scale = entity.EntityData.GetPropertyCoordinate("scale", Coordinate.One).VectorMagnitude();
                return (int)(scale * dist);
            }
            else
            {
                return int.MaxValue;
            }
        }
    }
}
