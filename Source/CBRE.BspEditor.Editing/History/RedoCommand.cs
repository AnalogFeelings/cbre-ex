using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CBRE.BspEditor.Commands;
using CBRE.BspEditor.Documents;
using CBRE.BspEditor.Editing.Properties;
using CBRE.BspEditor.Modification;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Hotkeys;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.History
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [CommandID("BspEditor:Edit:Redo")]
    [DefaultHotkey("Ctrl+Y")]
    [MenuItem("Edit", "", "History", "D")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_Redo))]
    public class RedoCommand : BaseCommand
    {
        public override string Name { get; set; } = "Redo";
        public override string Details { get; set; } = "Redo the last undone operation";

        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            HistoryStack stack = document.Map.Data.GetOne<HistoryStack>();
            if (stack == null) return;
            if (stack.CanRedo()) await MapDocumentOperation.Perform(document, stack.RedoOperation());
        }
    }
}