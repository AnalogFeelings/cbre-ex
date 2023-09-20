using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows.Forms;
using CBRE.Common.Shell.Commands;
using CBRE.Common.Shell.Context;
using CBRE.Common.Shell.Menu;
using CBRE.Common.Translations;
using CBRE.Shell.Forms;
using CBRE.Shell.Properties;

namespace CBRE.Shell.Commands
{
    /// <summary>
    /// Opens the translator form
    /// </summary>
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [CommandID("Tools:Translator")]
    [MenuItem("Tools", "", "Settings", "B")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_Translate))]
    [AllowToolbar(false)]
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
                TranslationForm tf = new TranslationForm();
                tf.Show(_shell);
            });
            return Task.CompletedTask;
        }
    }
}
