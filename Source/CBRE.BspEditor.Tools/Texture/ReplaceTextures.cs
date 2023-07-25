using System.ComponentModel.Composition;
using System.Threading.Tasks;
using LogicAndTrick.Oy;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Tools.Properties;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Tools.Texture
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [CommandID("BspEditor:ReplaceTextures")]
    [MenuItem("Tools", "", "Texture", "B")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_ReplaceTextures))]
    public class ReplaceTextures : ICommand
    {
        [Import] private ITranslationStringProvider _translation;

        public string Name { get; set; } = "Replace textures...";
        public string Details { get; set; } = "Replace textures";

        public bool IsInContext(IContext context)
        {
            return context.TryGet("ActiveDocument", out MapDocument _);
        }

        public async Task Invoke(IContext context, CommandParameters parameters)
        {
            MapDocument md = context.Get<MapDocument>("ActiveDocument");
            if (md == null) return;

            await Oy.Publish("Context:Add", new ContextInfo("BspEditor:TextureReplace"));
        }
    }
}
