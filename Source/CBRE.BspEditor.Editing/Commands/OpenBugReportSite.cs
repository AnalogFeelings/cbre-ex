using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CBRE.BspEditor.Editing.Properties;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;

namespace CBRE.BspEditor.Editing.Commands
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [MenuItem("Help", "", "Links", "D")]
    [CommandID("BspEditor:Links:BugReport")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_GitHub))]
    public class OpenBugReportSite : ICommand
    {
        public string Name { get; set; } = "Report a bug...";
        public string Details { get; set; } = "Report a bug in the GitHub repository.";

        public bool IsInContext(IContext context)
        {
            return true;
        }

        public async Task Invoke(IContext context, CommandParameters parameters)
        {
            System.Diagnostics.Process.Start("https://github.com/AnalogFeelings/cbre-ex/issues/new?assignees=AnalogFeelings&labels=bug&template=bug_report.md&title=");
        }
    }
}