using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace CBRE.Extended.Editor.Logging;

public class ExceptionInfo
{
    public Exception Exception { get; set; }
    public string RuntimeVersion { get; set; }
    public string OperatingSystem { get; set; }
    public string ApplicationVersion { get; set; }
    public string ProcessorName { get; set; }
    public string WorkingSet { get; set; }
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

    private string GetSystemName()
    {
        string osName = string.Empty;
        Version version = Environment.OSVersion.Version;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            switch (version.Major)
            {
                case 6:
                    osName = version.Minor switch
                    {
                        1 => "Windows 7",
                        2 => "Windows 8",
                        3 => "Windows 8.1",
                        _ => "Unknown Windows",
                    };
                    break;
                case 10:
                    switch (version.Minor)
                    {
                        case 0:
                            if (version.Build >= 22000) osName = "Windows 11";
                            else osName = "Windows 10";

                            break;
                        default:
                            osName = "Unknown Windows";
                            break;
                    }

                    break;
                default:
                    osName = "Unknown Windows";
                    break;
            }
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            switch (version.Major)
            {
                case 10:
                    switch (version.Minor)
                    {
                        case 15:
                            osName = "macOS Catalina";
                            break;
                        default:
                            osName = "Unknown macOS";
                            break;
                    }

                    break;
                case 11:
                    osName = "macOS Big Sur";
                    break;
                case 12:
                    osName = "macOS Monterey";
                    break;
                case 13:
                    osName = "macOS Ventura";
                    break;
                default:
                    osName = "Unknown macOS";
                    break;
            }
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            if (File.Exists("/etc/issue.net"))
                osName = File.ReadAllText("/etc/issue.net");
            else
                osName = "Linux";
        }
        else
        {
            osName = "Unknown OS";
        }
        
        osName += $" (Version {version.Major}.{version.Minor}, Build {version.Build})";

        return osName;
    }

    public ExceptionInfo(Exception Exception, string Information)
    {
        this.Exception = Exception;
        RuntimeVersion = Environment.Version.ToString();
        Date = DateTime.Now;
        InformationMessage = Information;
        ApplicationVersion = Editor.Version.ToString(3);
        OperatingSystem = GetSystemName();

        try
        {
            // TODO: Make a cross-platform way to check for CPU name.
            using (RegistryKey Key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0\"))
            {
                ProcessorName = Key.GetValue("ProcessorNameString").ToString().Trim();
            }
        }
        catch (Exception)
        {
            ProcessorName = "Unknown Processor";
        }
        
        WorkingSet = Process.GetCurrentProcess().PrivateMemorySize64 / 1000000 + "MB";

        List<Exception> list = new List<Exception>();
        do
        {
            list.Add(Exception);
            Exception = Exception.InnerException;
        } while (Exception != null);

        FullStackTrace = (Information + "\r\n").Trim();
        foreach (Exception ex in Enumerable.Reverse(list))
        {
            FullStackTrace += "\r\n" + ex.Message + " (" + ex.GetType().FullName + ")\r\n" + ex.StackTrace;
        }

        FullStackTrace = FullStackTrace.Trim();
    }
}