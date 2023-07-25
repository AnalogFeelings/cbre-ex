using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CBRE.Shell.Controls
{
    public class ReadOnlyTextBox : TextBox
    {
        [DllImport("user32.dll")]
        private static extern bool HideCaret(IntPtr hWnd);

        public ReadOnlyTextBox()
        {
            this.ReadOnly = true;

            this.GotFocus += OnGotFocus;
        }

        private void OnGotFocus(object sender, EventArgs e)
        {
            HideCaret(this.Handle);
        }
    }
}
