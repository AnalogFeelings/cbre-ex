using CBRE.Common.Native;
using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CBRE.Shell.Forms
{
    public partial class ExceptionWindow : Form
    {
        public ExceptionInfo Info { get; set; }

        private string LogText { get; set; }

        public ExceptionWindow(Exception ex)
        {
            InitializeComponent();

            ExceptionInfo info = new ExceptionInfo(ex, string.Empty);

            LogText = "CBRE-EX has encountered an error it couldn't recover from. Details are found below.\n" +
                      "-----------------------------------------------------------------------------------\n" +
                      $"System Processor: {info.ProcessorName}\n" +
                      $"Available Memory: {info.AvailableMemory}\n" +
                      $"Operating System: {info.OperatingSystem}\n" +
                      $".NET Version: {info.RuntimeVersion}\n" +
                      $"CBRE-EX Version: {info.ApplicationVersion}\n" +
                      "-----------------------------------ERROR MESSAGE-----------------------------------\n" +
                      info.FullStackTrace;

            Info = info;

            processorName.Text = info.ProcessorName;
            availableMemory.Text = info.AvailableMemory;
            runtimeVersion.Text = info.RuntimeVersion;
            operatingSystem.Text = info.OperatingSystem;
            editorVersion.Text = info.ApplicationVersion;
            fullError.Text = info.FullStackTrace;

            StockIconInfo stockIconInfo = new StockIconInfo();
            stockIconInfo.cbSize = (uint)Marshal.SizeOf(typeof(StockIconInfo));
            StockIcon.SHGetStockIconInfo(StockIconId.SIID_ERROR, StockIconFlags.SHGSI_ICON | StockIconFlags.SHGSI_SHELLICONSIZE, ref stockIconInfo);

            systemBitmap.Image = Icon.FromHandle(stockIconInfo.hIcon).ToBitmap();

            try
            {
                Directory.CreateDirectory(@"Logs\Exceptions");
                string fn = DateTime.Now.ToString("dd-MM-yy-HH-mm-ss");

                using (StreamWriter streamWriter = new StreamWriter($"Logs\\Exceptions\\{fn}.txt"))
                {
                    streamWriter.Write(LogText);
                }

                headerLabel.Text += $"Information has been written to \"Logs\\Exceptions\\{fn}.txt\".";
            }
            catch (Exception e)
            {
                headerLabel.Text += $"Couldn't write error log: {e.Message}.";
            }

            fullError.SelectionLength = 0;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(LogText);
        }

        private void reportButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/AnalogFeelings/cbre-ex/issues/new?assignees=AnalogFeelings&labels=bug&template=bug_report.md&title=");
        }

        public class ExceptionInfo
        {
            public Exception Exception { get; set; }
            public string RuntimeVersion { get; set; }
            public string OperatingSystem { get; set; }
            public string ApplicationVersion { get; set; }
            public string ProcessorName { get; set; }
            public string AvailableMemory { get; set; }
            public DateTime Date { get; set; }
            public string InformationMessage { get; set; }

            public string Source => Exception.Source;

            public string Message
            {
                get
                {
                    string msg = string.IsNullOrWhiteSpace(InformationMessage) ? Exception.Message : InformationMessage;

                    return msg.Split('\n').Select(x => x.Trim()).FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
                }
            }

            public string StackTrace => Exception.StackTrace;

            public string FullStackTrace { get; set; }

            public ExceptionInfo(Exception exception, string info)
            {
                Exception = exception;
                RuntimeVersion = Environment.Version.ToString();
                Date = DateTime.Now;
                InformationMessage = info;
                ApplicationVersion = Assembly.GetAssembly(typeof(Shell)).GetName().Version.ToString(3);
                OperatingSystem = FriendlyOSName();

                try
                {
                    using (RegistryKey Key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0\"))
                    {
                        ProcessorName = Key.GetValue("ProcessorNameString").ToString().Trim();
                    }

                    AvailableMemory = new ComputerInfo().AvailablePhysicalMemory / 1000000 + "MB";
                }
                catch (Exception)
                {
                    ProcessorName = "Unknown Processor";
                    AvailableMemory = "Unknown";
                }

                List<Exception> list = new List<Exception>();
                do
                {
                    list.Add(exception);
                    exception = exception.InnerException;
                } while (exception != null);

                FullStackTrace = (info + "\r\n").Trim();
                foreach (Exception ex in Enumerable.Reverse(list))
                {
                    FullStackTrace += "\r\n" + ex.Message + " (" + ex.GetType().FullName + ")\r\n" + ex.StackTrace;
                }
                FullStackTrace = FullStackTrace.Trim();
            }

            public string FriendlyOSName()
            {
                Version version = Environment.OSVersion.Version;
                string os;

                switch (version.Major)
                {
                    case 6:
                        switch (version.Minor)
                        {
                            case 1: os = $"Windows 7"; break;
                            case 2: os = $"Windows 8"; break;
                            case 3: os = $"Windows 8.1"; break;
                            default: os = "Unknown"; break;
                        }
                        break;
                    case 10:
                        switch (version.Minor)
                        {
                            case 0:
                                if (version.Build >= 22000) os = $"Windows 11";
                                else os = $"Windows 10";
                                break;
                            default: os = "Unknown"; break;
                        }
                        break;
                    default:
                        os = "Unknown";
                        break;
                }

                os += $" (NT {version.Major}.{version.Minor}, Build {version.Build})";

                return os;
            }
        }
    }
}
