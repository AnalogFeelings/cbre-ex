using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CBRE.BspEditor.Commands;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Editing.Properties;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.Commands.Pointfile
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [MenuItem("Map", "", "Pointfile", "F")]
    [CommandID("BspEditor:Map:UnloadPointfile")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_UnloadPointfile))]
    public class UnloadPointfile : BaseCommand
    {
        public override string Name { get; set; } = "Unload pointfile...";
        public override string Details { get; set; } = "Clear the currently loaded pointfile";

        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            Pointfile pf = document.Map.Data.GetOne<Pointfile>();
            if (pf == null) return;

            await MapDocumentOperation.Perform(document, new TrivialOperation(
                d => d.Map.Data.Remove(pf),
                c => c.Add(c.Document.Map.Root)
            ));
        }
    }
}