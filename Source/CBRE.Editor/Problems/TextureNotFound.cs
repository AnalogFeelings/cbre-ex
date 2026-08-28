using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Actions;
using CBRE.Editor.Actions.MapObjects.Operations;
using CBRE.Localization;
using System.Collections.Generic;
using System.Linq;

namespace CBRE.Editor.Problems
{
    public class TextureNotFound : IProblemCheck
    {
        public IEnumerable<Problem> Check(Map map, bool visibleOnly)
        {
            List<Face> faces = map.WorldSpawn
                .Find(x => x is Solid && (!visibleOnly || (!x.IsVisgroupHidden && !x.IsCodeHidden)))
                .OfType<Solid>()
                .SelectMany(x => x.Faces)
                .Where(x => x.Texture.Texture == null)
                .ToList();
            foreach (string name in faces.Select(x => x.Texture.Name).Distinct())
            {
                yield return new Problem(GetType(), map, faces.Where(x => x.Texture.Name == name).ToList(), Fix, Local.LocalString("document.texture_not_found", name), Local.LocalString("document.texture_not_found.description"));
            }
        }

        public IAction Fix(Problem problem)
        {
            return new EditFace(problem.Faces, (d, x) =>
            {
                char[] ignored = "{#!~+-0123456789".ToCharArray();
                Providers.Texture.TextureItem def = d.TextureCollection.GetAllBrowsableItems()
                    .OrderBy(i => new string(i.Name.Where(c => !ignored.Contains(c)).ToArray()) + "Z")
                    .FirstOrDefault();
                if (def != null)
                {
                    x.Texture.Name = def.Name;
                    x.Texture.Texture = def.GetTexture();
                    x.CalculateTextureCoordinates(true);
                }
            }, true);
        }
    }
}
