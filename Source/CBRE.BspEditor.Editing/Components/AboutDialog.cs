using CBRE.BspEditor.Editing.Properties;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace CBRE.BspEditor.Editing.Components
{
    public partial class AboutDialog : Form
    {
        // TODO: Add translation support.
        public AboutDialog()
        {
            InitializeComponent();

            Icon = Icon.FromHandle(Resources.Menu_Help.GetHicon());

            VersionLabel.Text = VersionLabel.Text.Replace("(version)", Assembly.GetAssembly(typeof(AboutDialog)).GetName().Version.ToString(3));

            CreditsBox.SelectAll();
            CreditsBox.SelectionAlignment = HorizontalAlignment.Center;
            CreditsBox.DeselectAll();

            CreditsBox.LinkClicked += (s, e) => OpenSite(e.LinkText);
        }

        private void OpenSite(string url)
        {
            Process.Start(url);
        }
    }
}
