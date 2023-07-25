using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using CBRE.BspEditor.Commands;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Editing.Properties;
using CBRE.BspEditor.Modification;
using CBRE.BspEditor.Modification.Operations.Data;
using CBRE.BspEditor.Primitives.MapObjectData;
using CBRE.BspEditor.Primitives.MapObjects;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Hotkeys;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.Commands.Quick
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [MenuItem("View", "", "Quick", "D")]
    [CommandID("BspEditor:View:QuickHideUnselected")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_HideUnselected))]
    [DefaultHotkey("Ctrl+H")]
    public class HideUnselectedObjects : BaseCommand
    {
        public override string Name { get; set; } = "Quick hide unselected";
        public override string Details { get; set; } = "Quick hide unselected objects";

        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            Transaction transaction = new Transaction();

            foreach (IMapObject mo in document.Map.Root.FindAll().Except(document.Selection).Where(x => !(x is Root)).ToList())
            {
                QuickHidden ex = mo.Data.GetOne<QuickHidden>();
                if (ex != null) transaction.Add(new RemoveMapObjectData(mo.ID, ex));
                transaction.Add(new AddMapObjectData(mo.ID, new QuickHidden()));
            }

            await MapDocumentOperation.Perform(document, transaction);
        }
    }
}