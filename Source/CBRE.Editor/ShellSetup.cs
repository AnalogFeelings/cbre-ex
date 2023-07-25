using System.ComponentModel.Composition;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using CBRE.Common.Shell.Hooks;
using CBRE.Common.Translations;
using CBRE.Editor.Properties;
using CBRE.Shell;

namespace CBRE.Editor
{
    [Export(typeof(IInitialiseHook))]
    [AutoTranslate]
    public class ShellSetup : IInitialiseHook
    {
        private readonly Form _shell;

        public string Title { get; set; }
        public string Version { get; set; }

        [ImportingConstructor]
        public ShellSetup([Import("Shell")] Form shell)
        {
            _shell = shell;

            Version = Assembly.GetAssembly(typeof(ShellSetup)).GetName().Version.ToString(3);
        }

        public Task OnInitialise()
        {
            _shell.InvokeLater(() =>
            {
                _shell.Icon = Resources.CBRE;
                _shell.Text = string.Format(Title, Version);

                PropertyInfo prop = _shell.GetType().GetProperty("Title");
                if (prop != null)
                {
                    prop.SetValue(_shell, Title);
                }
            });

            return Task.CompletedTask;
        }
    }
}
