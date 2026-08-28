using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CBRE.Localization;
using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;

namespace CBRE.Editor.Logging
{
    public static class Logger
    {
        public static void ShowException(Exception ex, string message = "")
        {
            ExceptionInfo info = new ExceptionInfo(ex, message);
            ExceptionWindow window = new ExceptionWindow(info);
            if (Editor.Instance == null || Editor.Instance.IsDisposed) window.Show();
            else window.Show(Editor.Instance);
        }
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
        public string UserEnteredInformation { get; set; }

        public string Source
        {
            get { return Exception.Source; }
        }

        public string Message
        {
            get
            {
                string msg = String.IsNullOrWhiteSpace(InformationMessage) ? Exception.Message : InformationMessage;
                return msg.Split('\n').Select(x => x.Trim()).FirstOrDefault(x => !String.IsNullOrWhiteSpace(x));
            }
        }

        public string StackTrace
        {
            get { return Exception.StackTrace; }
        }

        public string FullStackTrace { get; set; }

        public string FriendlyOSName()
        {
            Version version = System.Environment.OSVersion.Version;
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
                        default: os = Local.LocalString("info.os.unknown"); break;
                    }
                    break;
                default:
                    os = Local.LocalString("info.os.unknown");
                    break;
            }
            os += $" (NT {version.Major}.{version.Minor}, Build {version.Build})";
            return os;
        }

        public ExceptionInfo(Exception exception, string info)
        {
            Exception = exception;
            RuntimeVersion = System.Environment.Version.ToString();
            Date = DateTime.Now;
            InformationMessage = info;
            ApplicationVersion = Assembly.GetAssembly(typeof(Editor)).GetName().Version.ToString(3);
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
                ProcessorName = Local.LocalString("info.processor.unknown");
                AvailableMemory = Local.LocalString("info.avail_memory.unknown");
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
    }
}
