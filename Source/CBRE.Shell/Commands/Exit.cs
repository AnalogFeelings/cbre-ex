using System.ComponentModel.Composition;
using System.Threading.Tasks;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Hotkeys;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;
using CBRE.Shell.Properties;

namespace CBRE.Shell.Commands
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [CommandID("File:Exit")]
    [DefaultHotkey("Alt+F4")]
    [MenuItem("File", "", "Exit", "M")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_Exit))]
    public class Exit : ICommand
    {
        private readonly Forms.Shell _shell;

        public string Name { get; set; } = "Exit";
        public string Details { get; set; } = "Exit";

        [ImportingConstructor]
        internal Exit([Import] Forms.Shell shell)
        {
            _shell = shell;
        }

        public bool IsInContext(IContext context)
        {
            return true;
        }

        public Task Invoke(IContext context, CommandParameters parameters)
        {
            _shell.Close();
            return Task.CompletedTask;
        }
    }
}