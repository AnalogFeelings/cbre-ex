using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CBRE.Common.Mediator;
using CBRE.Localization;
using CBRE.UI.Native;

namespace CBRE.Editor.Logging
{
    public partial class ExceptionWindow : Form
    {
        public ExceptionInfo ExceptionInfo { get; set; }
        private string LogText { get; set; }

        public ExceptionWindow(ExceptionInfo info)
        {
            ExceptionInfo = info;
            
            LogText = Local.LocalString("error.cant_recover") + "\n" +
                      "-----------------------------------------------------------------------------------\n" +
                      Local.LocalString("info.processor", $"System Processor: {info.ProcessorName}\n") +
                      Local.LocalString("info.avail_memory", $"{info.AvailableMemory}\n") +
                      Local.LocalString("info.os", $"{info.OperatingSystem}\n") +
                      Local.LocalString("info.dotnet", $"{info.RuntimeVersion}\n") +
                      Local.LocalString("info.version", $"{info.ApplicationVersion}\n") +
                      "-----------------------------------" + Local.LocalString("error.message") + "-----------------------------------\n" +
                      info.FullStackTrace;
            
            InitializeComponent();
            
            ProcessorName.Text = info.ProcessorName;
            AvailableMemory.Text = info.AvailableMemory;
            RuntimeVersion.Text = info.RuntimeVersion;
            OperatingSystem.Text = info.OperatingSystem;
            CBREVersion.Text = info.ApplicationVersion;
            FullError.Text = info.FullStackTrace;
            
            NativeIcons.SHSTOCKICONINFO StockIconInfo = new NativeIcons.SHSTOCKICONINFO();
            StockIconInfo.cbSize = (UInt32)Marshal.SizeOf(typeof(NativeIcons.SHSTOCKICONINFO));
            NativeIcons.SHGetStockIconInfo(NativeIcons.SHSTOCKICONID.SIID_ERROR, NativeIcons.SHGSI.SHGSI_ICON | NativeIcons.SHGSI.SHGSI_SHELLICONSIZE, ref StockIconInfo);

            systemBitmap.Image = Icon.FromHandle(StockIconInfo.hIcon).ToBitmap();
            
            try
            {
                Directory.CreateDirectory("Logs\\Exceptions");
                string fn = DateTime.Now.ToString("dd-MM-yy-HH-mm-ss");
                using (StreamWriter sw = new StreamWriter($"Logs\\Exceptions\\{fn}.txt"))
                {
                    sw.Write(LogText);
                }
                HeaderLabel.Text += Local.LocalString("log.Information_written_to", $"\"Logs\\Exceptions\\{fn}.txt\"");
            }
            catch (Exception e)
            {
                HeaderLabel.Text += Local.LocalString("log.could_not_write_errorlog", e.Message);
            }

            FullError.SelectionLength = 0;
        }

        private void CancelButtonClicked(object sender, EventArgs e)
        {
            Close();
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(LogText);
        }

        private void reportButton_Click(object sender, EventArgs e)
        {
            Mediator.Publish(EditorMediator.OpenWebsite, Editor.GITHUB_REPORT_BUG_URL);
        }
    }
}
