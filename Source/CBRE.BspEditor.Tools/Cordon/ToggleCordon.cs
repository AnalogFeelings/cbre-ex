using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations;
using CBRE.BspEditor.Primitives.MapData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.BspEditor.Tools.Properties;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Tools.Cordon
{
    [Export(typeof(ICommand))]
    [CommandID("BspEditor:Cordon:ToggleCordon")]
    [MenuItem("Tools", "", "Cordon", "B")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_Cordon))]
    [AutoTranslate]
    public class ToggleCordon : ICommand
    {
        public string Name { get; set; } = "Cordon Bounds";
        public string Details { get; set; } = "Toggle cordon bounds";

        public bool IsInContext(IContext context)
        {
            return context.TryGet("ActiveDocument", out MapDocument _);
        }

        public async Task Invoke(IContext context, CommandParameters parameters)
        {
            if (context.TryGet("ActiveDocument", out MapDocument doc))
            {
                var cordon = doc.Map.Data.GetOne<CordonBounds>() ?? new CordonBounds {Enabled = false};
                cordon.Enabled = !cordon.Enabled;
                await MapDocumentOperation.Perform(doc, new TrivialOperation(x => x.Map.Data.Replace(cordon), x => x.Update(cordon).UpdateRange(doc.Map.Root.FindAll())));
            }
        }
    }
}