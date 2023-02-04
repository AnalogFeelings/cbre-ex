using System.ComponentModel.Composition;
using System.Threading.Tasks;
using LogicAndTrick.Oy;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Hotkeys;
using CBRE.Common.Translations;

namespace CBRE.Shell.Commands
{
    /// <summary>
    /// Opens the command box
    /// </summary>
    [Export(typeof(ICommand))]
    [DefaultHotkey("Ctrl+T")]
    [AutoTranslate]
    public class OpenCommandBox : ICommand
    {
        public string Name { get; set; } = "Open the command box";
        public string Details { get; set; } = "Open the command box";

        public bool IsInContext(IContext context)
        {
            return true;
        }

        public async Task Invoke(IContext context, CommandParameters parameters)
        {
            await Oy.Publish<string>("Shell:OpenCommandBox", "");
        }
    }
}
