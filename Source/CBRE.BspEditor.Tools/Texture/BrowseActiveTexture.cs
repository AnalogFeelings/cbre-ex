using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows.Forms;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Tools.Texture
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [CommandID("BspEditor:BrowseActiveTexture")]
    public class BrowseActiveTexture : ICommand
    {
        [Import] private Lazy<ITranslationStringProvider> _translation;

        public string Name { get; set; } = "Open texture browser";
        public string Details { get; set; } = "Open texture browser";

        public bool IsInContext(IContext context)
        {
            return context.TryGet("ActiveDocument", out MapDocument _);
        }

        public async Task Invoke(IContext context, CommandParameters parameters)
        {
            MapDocument md = context.Get<MapDocument>("ActiveDocument");
            if (md == null) return;
            using (TextureBrowser tb = new TextureBrowser(md))
            {
                await tb.Initialise(_translation.Value);
                if (tb.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(tb.SelectedTexture))
                {
                    string tex = tb.SelectedTexture;
                    ActiveTexture at = new ActiveTexture {Name = tex};
                    MapDocumentOperation.Perform(md, new TrivialOperation(x => x.Map.Data.Replace(at), x => x.Update(at)));
                }
            }
        }
    }
}
