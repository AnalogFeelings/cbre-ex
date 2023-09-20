using CBRE.BspEditor.Editing.Properties;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace CBRE.BspEditor.Editing.Components
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();

            Icon = Icon.FromHandle(Resources.Menu_Help.GetHicon());

            VersionLabel.Text = VersionLabel.Text.Replace("(version)", Assembly.GetAssembly(typeof(AboutDialog)).GetName().Version.ToString(3));

            GitHubLink.Click += (s, e) => OpenSite(GitHubLink.Text);
            LicenseLink.Click += (s, e) => OpenSite(LicenseLink.Text);
            ExtraLicenseLink.Click += (s, e) => OpenSite(ExtraLicenseLink.Text);

            DescriptionLabel.Links.Add(214, 19, "http://logic-and-trick.com");
            DescriptionLabel.LinkClicked += (s, e) => OpenSite(e.Link.LinkData.ToString());
        }

        private void OpenSite(string url)
        {
            Process.Start(url);
        }
    }
}
