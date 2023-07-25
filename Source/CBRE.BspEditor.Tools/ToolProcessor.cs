using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Providers.Processors;

namespace CBRE.BspEditor.Tools
{
    [Export(typeof(IBspSourceProcessor))]
    public class ToolProcessor : IBspSourceProcessor
    {
        public string OrderHint => "B";

        public async Task AfterLoad(MapDocument document)
        {
            if (!document.Map.Data.Any(x => x is ActiveTexture))
            {
                Environment.TextureCollection tc = await document.Environment.GetTextureCollection();
                string first = tc.GetBrowsableTextures()
                    .OrderBy(t => t, StringComparer.CurrentCultureIgnoreCase)
                    .Where(item => item.Length > 0)
                    .Select(item => new { item, c = char.ToLower(item[0]) })
                    .Where(t => t.c >= 'a' && t.c <= 'z')
                    .Select(t => t.item)
                    .FirstOrDefault();
                document.Map.Data.Add(new ActiveTexture { Name = first });
            }
        }

        public Task BeforeSave(MapDocument document)
        {
            return Task.FromResult(0);
        }
    }
}