using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows.Forms;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;
using CBRE.Shell.Forms;

namespace CBRE.Shell.Commands
{
    /// <summary>
    /// Opens the translator form
    /// </summary>
    [Export(typeof(ICommand))]
    [CommandID("Tools:Translator")]
    [MenuItem("Tools", "", "Settings", "B")]
    [AutoTranslate]
    public class OpenTranslator : ICommand
    {
        private readonly Form _shell;

        public string Name { get; set; } = "CBRE-EX translator...";
        public string Details { get; set; } = "Open the translator app";

        [ImportingConstructor]
        public OpenTranslator([Import("Shell")] Form shell)
        {
            _shell = shell;
        }

        public bool IsInContext(IContext context)
        {
            return true;
        }

        public Task Invoke(IContext context, CommandParameters parameters)
        {
            _shell.InvokeLater(() =>
            {
                var tf = new TranslationForm();
                tf.Show(_shell);
            });
            return Task.CompletedTask;
        }
    }
}
