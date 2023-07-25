using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations.Data;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.Problems
{
    [Export(typeof(IProblemCheck))]
    [AutoTranslate]
    public class TextureNotFound : IProblemCheck
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public Uri Url => null;
        public bool CanFix => true;

        public async Task<List<Problem>> Check(MapDocument document, Predicate<IMapObject> filter)
        {
            Environment.TextureCollection tc = await document.Environment.GetTextureCollection();

            // Get a list of all faces and textures
            var faces = document.Map.Root.FindAll()
                .OfType<Solid>()
                .Where(x => filter(x))
                .SelectMany(x => x.Faces.Select(f => new {Object = x, Face = f}))
                .ToList();

            // Get the list of textures in the map and in the texture collection
            HashSet<string> textureNames = faces.Select(x => x.Face.Texture.Name).ToHashSet(StringComparer.InvariantCultureIgnoreCase);
            HashSet<string> knownTextureNames = tc.GetAllTextures().ToHashSet(StringComparer.InvariantCultureIgnoreCase);

            // The set only contains textures that aren't known
            textureNames.ExceptWith(knownTextureNames);

            return faces
                .Where(x => textureNames.Contains(x.Face.Texture.Name))
                .Select(x => new Problem {Text = x.Face.Texture.Name}.Add(x.Object).Add(x.Face))
                .ToList();
        }

        public async Task Fix(MapDocument document, Problem problem)
        {
            Environment.TextureCollection tc = await document.Environment.GetTextureCollection();

            // Get the default texture to apply
            string first = tc.GetBrowsableTextures()
                .OrderBy(t => t, StringComparer.CurrentCultureIgnoreCase)
                .Where(item => item.Length > 0)
                .Select(item => new { item, c = char.ToLower(item[0]) })
                .Where(t => t.c >= 'a' && t.c <= 'z')
                .Select(t => t.item)
                .FirstOrDefault();

            Transaction transaction = new Transaction();

            foreach (IMapObject obj in problem.Objects)
            {
                foreach (Face face in obj.Data.Intersect(problem.ObjectData).OfType<Face>())
                {
                    Face clone = (Face)face.Clone();
                    clone.Texture.Name = first;

                    transaction.Add(new RemoveMapObjectData(obj.ID, face));
                    transaction.Add(new AddMapObjectData(obj.ID, clone));
                }
            }

            await MapDocumentOperation.Perform(document, transaction);
        }
    }
}