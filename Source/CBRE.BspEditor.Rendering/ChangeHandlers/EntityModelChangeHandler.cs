using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.ChangeHandling;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.BspEditor.Rendering.ResourceManagement;
using CBRE.DataStructures.GameData;
using CBRE.Rendering.Interfaces;

namespace CBRE.BspEditor.Rendering.ChangeHandlers
{
    [Export(typeof(IMapDocumentChangeHandler))]
    public class EntityModelChangeHandler : IMapDocumentChangeHandler
    {
        private readonly Lazy<ResourceCollection> _resourceCollection;

        public string OrderHint => "M";

        [ImportingConstructor]
        public EntityModelChangeHandler(
            [Import] Lazy<ResourceCollection> resourceCollection
        )
        {
            _resourceCollection = resourceCollection;
        }

        public async Task Changed(Change change)
        {
            GameData gd = await change.Document.Environment.GetGameData();
            foreach (Entity entity in change.Added.Union(change.Updated).OfType<Entity>())
            {
                ModelDetails modelDetails = GetModelDetails(entity, gd);
                string modelName = modelDetails?.Name;
                EntityModel existingEntityModel = entity.Data.GetOne<EntityModel>();

                // If the model data is unchanged then we can skip
                if (ModelDataMatches(existingEntityModel, modelDetails))
                {
                    if (existingEntityModel != null)
                    {
                        UpdateSequence(existingEntityModel, modelDetails);
                        entity.DescendantsChanged();
                    }
                    continue;
                }

                // Load the model if the name is specified
                // This doesn't cause unnecessary load as if the model is already loaded then
                // nothing will happen, and otherwise we need to load the model anyway.
                IModel model = null;
                if (!string.IsNullOrWhiteSpace(modelName))
                {
                    model = await _resourceCollection.Value.GetModel(change.Document.Environment, modelName);
                    if (model == null) modelName = null;
                }

                // If there's no model then we need to remove the entity model if it exists
                if (model == null)
                {
                    if (entity.Data.Remove(x => x is EntityModel) > 0) entity.DescendantsChanged();
                    continue;
                }

                IModelRenderable renderable = _resourceCollection.Value.CreateModelRenderable(change.Document.Environment, model);
                EntityModel sd = new EntityModel(modelName, renderable);
                UpdateSequence(sd, modelDetails);

                entity.Data.Replace(sd);
                entity.DescendantsChanged();
            }

            // Ensure removed entity models are disposed properly
            foreach (IMapObject rem in change.Removed)
            {
                EntityModel em = rem.Data.GetOne<EntityModel>();
                if (em?.Renderable == null) continue;

                _resourceCollection.Value.DestroyModelRenderable(change.Document.Environment, em.Renderable);
                rem.Data.Remove(em);
            }
        }

        private void UpdateSequence(EntityModel entityModel, ModelDetails modelDetails)
        {
            if (modelDetails == null || entityModel.Renderable == null) return;

            System.Collections.Generic.List<string> sequences = entityModel.Renderable.Model.GetSequences();
            int seq = modelDetails.Sequence;
            if (seq >= sequences.Count) seq = -1;

            // Find the default sequence if one isn't set
            if (seq < 0) seq = sequences.IndexOf("idle");
            if (seq < 0) seq = sequences.FindIndex(x => x.StartsWith("idle", StringComparison.InvariantCultureIgnoreCase));
            if (seq < 0) seq = 0;
            
            entityModel.Renderable.Sequence = seq;

            entityModel.Renderable.Origin = modelDetails.Origin;
            entityModel.Renderable.Angles = modelDetails.Angles;
        }

        private bool ModelDataMatches(EntityModel model, ModelDetails details)
        {
            string name = details?.Name;
            return string.IsNullOrWhiteSpace(name)
                ? model == null
                : string.Equals(model?.Name, name, StringComparison.InvariantCultureIgnoreCase) && model?.Renderable != null;
        }

        private static ModelDetails GetModelDetails(Entity entity, GameData gd)
        {
            if (entity.Hierarchy.HasChildren || string.IsNullOrWhiteSpace(entity.EntityData.Name)) return null;
            GameDataObject cls = gd?.GetClass(entity.EntityData.Name);
            if (cls == null) return null;

            Behaviour studio = cls.Behaviours.FirstOrDefault(x => string.Equals(x.Name, "studio", StringComparison.InvariantCultureIgnoreCase))
                         ?? cls.Behaviours.FirstOrDefault(x => string.Equals(x.Name, "sprite", StringComparison.InvariantCultureIgnoreCase));
            if (studio == null) return null;

            ModelDetails details = new ModelDetails();

            // First see if the studio behaviour forces a model...
            if (studio.Values.Count == 1 && !string.IsNullOrWhiteSpace(studio.Values[0]))
            {
                details.Name = studio.Values[0].Trim();
            }
            else
            {
                // Find the first property that is a studio type, or has a name of "model"...
                Property prop = cls.Properties.FirstOrDefault(x => x.VariableType == VariableType.Studio) ??
                           cls.Properties.FirstOrDefault(x => string.Equals(x.Name, "model", StringComparison.InvariantCultureIgnoreCase));
                if (prop != null)
                {
                    string val = entity.EntityData.Get(prop.Name, prop.DefaultValue);
                    if (!string.IsNullOrWhiteSpace(val)) details.Name = val;
                }
            }

            if (details.Name != null)
            {
                Property seqProp = cls.Properties.FirstOrDefault(x => string.Equals(x.Name, "sequence", StringComparison.InvariantCultureIgnoreCase));
                string seqVal = entity.EntityData.Get("sequence", seqProp?.DefaultValue);
                if (!string.IsNullOrWhiteSpace(seqVal) && int.TryParse(seqVal, out int v)) details.Sequence = v;

                details.Origin = entity.Data.GetOne<Origin>()?.Location ?? entity.BoundingBox.Center;

                Vector3? ang = entity.EntityData.GetVector3("angles");
                if (ang.HasValue) details.Angles = ang.Value * (float) Math.PI / 180f;

                return details;
            }

            return null;
        }

        private class ModelDetails
        {
            public string Name { get; set; }
            public int Sequence { get; set; } = -1;
            public Vector3 Origin { get; set; }
            public Vector3 Angles { get; set; }
        }
    }
}
