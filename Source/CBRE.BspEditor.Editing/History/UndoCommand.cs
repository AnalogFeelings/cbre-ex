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
    [CommandID("BspEditor:Edit:Undo")]
    [DefaultHotkey("Ctrl+Z")]
    [MenuItem("Edit", "", "History", "B")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_Undo))]
    public class UndoCommand : BaseCommand
    {
        public override string Name { get; set; } = "Undo";
        public override string Details { get; set; } = "Undo the last operation";

        protected override async Task Invoke(MapDocument document, CommandParameters parameters)
        {
            HistoryStack stack = document.Map.Data.GetOne<HistoryStack>();
            if (stack == null) return;
            if (stack.CanUndo()) await MapDocumentOperation.Reverse(document, stack.UndoOperation());
        }
    }
}