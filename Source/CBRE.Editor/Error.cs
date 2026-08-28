using CBRE.Localization;
using System.Diagnostics;
using System.Windows.Forms;

namespace CBRE.Editor
{
    public static class Error
    {
        /// <summary>
        /// A critical error displays a message box and then closes the application.
        /// </summary>
        public static void Critical(string message)
        {
            MessageBox.Show(Local.LocalString("error.editor.critical", message), Local.LocalString("error.editor.critical.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }

        /// <summary>
        /// A warning displays a message box.
        /// </summary>
        public static void Warning(string message)
        {
            MessageBox.Show(Local.LocalString("error.editor.warning") + "\n\n" + message, Local.LocalString("error.editor.warning.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// A log writes the message to the debug output.
        /// </summary>
        public static void Log(string message)
        {
            Debug.WriteLine(Local.LocalString("error.editor.log", message));
        }
    }
}