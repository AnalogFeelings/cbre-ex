using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CBRE.Localization;
using CBRE.UI.Native;

namespace CBRE.Editor.Logging
{
    public partial class EntityErrorWindow : Form
    {
        public EntityErrorWindow(List<string> Errors)
        {
            InitializeComponent();

            NativeIcons.SHSTOCKICONINFO StockIconInfo = new NativeIcons.SHSTOCKICONINFO();
            StockIconInfo.cbSize = (UInt32)Marshal.SizeOf(typeof(NativeIcons.SHSTOCKICONINFO));
            NativeIcons.SHGetStockIconInfo(NativeIcons.SHSTOCKICONID.SIID_WARNING, NativeIcons.SHGSI.SHGSI_ICON | NativeIcons.SHGSI.SHGSI_SHELLICONSIZE, ref StockIconInfo);

            systemBitmap.Image = Icon.FromHandle(StockIconInfo.hIcon).ToBitmap();

            string joinedText = string.Join(System.Environment.NewLine, Errors);

            this.errorTextBox.Text += joinedText;

            try
            {
                Directory.CreateDirectory("Logs\\Entities");
                string filename = DateTime.Now.ToString("dd-MM-yy-HH-mm-ss") + ".txt";

                using (StreamWriter streamWriter = new StreamWriter($"Logs\\Entities\\{filename}"))
                {
                    string content = Local.LocalString("error.loading_custom_entites") + "\n" +
                                     "----------------------------------------------------------------------------------------\n" +
                                     joinedText;
                    streamWriter.Write(content);
                }

                logLabel.Text += Local.LocalString("log.details_written_to", $"\"Logs\\Entities\\{filename}\"");
            }
            catch (Exception ex)
            {
                logLabel.Text += Local.LocalString("log.could_not_write_errorlog", ex.Message);
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (dontShowAgainCheckbox.Checked) Editor.Instance.ShowEntityErrorForm = false;
            
            this.Close();
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(this.errorTextBox.Text);
        }
    }
}
