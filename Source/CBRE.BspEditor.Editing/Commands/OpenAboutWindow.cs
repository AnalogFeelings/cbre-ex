using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CBRE.BspEditor.Editing.Components;
using CBRE.BspEditor.Editing.Properties;
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
    [MenuImage(typeof(Resources), nameof(Resources.Menu_Help))]
    [AllowToolbar(false)]
    public class OpenAboutWindow : ICommand
    {
        public string Name { get; set; } = "About CBRE-EX";
        public string Details { get; set; } = "View information about this application";

        public bool IsInContext(IContext context)
        {
            return true;
        }

        public async Task Invoke(IContext context, CommandParameters parameters)
        {
            using (AboutDialog vg = new AboutDialog())
            {
                vg.ShowDialog();
            }
        }
    }
}