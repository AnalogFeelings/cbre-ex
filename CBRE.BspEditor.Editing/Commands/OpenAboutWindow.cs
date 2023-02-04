using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CBRE.BspEditor.Editing.Components;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.Commands
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [MenuItem("Help", "", "About", "Z")]
    [CommandID("BspEditor:Help:About")]
    public class OpenAboutWindow : ICommand
    {
        public string Name { get; set; } = "About CBRE";
        public string Details { get; set; } = "View information about this application";

        public bool IsInContext(IContext context)
        {
            return true;
        }

        public async Task Invoke(IContext context, CommandParameters parameters)
        {
            using (var vg = new AboutDialog())
            {
                vg.ShowDialog();
            }
        }
    }
}