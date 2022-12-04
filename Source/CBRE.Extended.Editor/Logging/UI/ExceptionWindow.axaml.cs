using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using TextCopy;

namespace CBRE.Extended.Editor.Logging.UI;

public partial class ExceptionWindow : Window
{
    private ExceptionInfo ExceptionInfo { get; set; }
    private string LogText { get; set; }

    public ExceptionWindow()
    {
        InitializeComponent();
    }

    public void SetUp(ExceptionInfo Information)
    {
        ExceptionInfo = Information;
        
        LogText = "CBRE-EX has encountered an error it couldn't recover from. Details are found below.\n" +
                  "-----------------------------------------------------------------------------------\n" +
                  $"System Processor: {ExceptionInfo.ProcessorName}\n" +
                  $"Working Set: {ExceptionInfo.WorkingSet}\n" +
                  $"Operating System: {ExceptionInfo.OperatingSystem}\n" +
                  $".NET Version: {ExceptionInfo.RuntimeVersion}\n" +
                  $"CBRE-EX Version: {ExceptionInfo.ApplicationVersion}\n" +
                  "-----------------------------------ERROR MESSAGE-----------------------------------\n" +
                  ExceptionInfo.FullStackTrace;

        SystemProcessor.Text = ExceptionInfo.ProcessorName;
        WorkingSet.Text = ExceptionInfo.WorkingSet;
        RuntimeVersion.Text = ExceptionInfo.RuntimeVersion;
        OperatingSystem.Text = ExceptionInfo.OperatingSystem;
        EditorVersion.Text = ExceptionInfo.ApplicationVersion;
        FullError.Text = ExceptionInfo.FullStackTrace;

        try
        {
            Directory.CreateDirectory("Logs\\Exceptions");
            string exceptionFilename = DateTime.Now.ToString("dd-MM-yy-HH-mm-ss");
            using (StreamWriter sw = new StreamWriter($"Logs\\Exceptions\\{exceptionFilename}.txt"))
            {
                sw.Write(LogText);
            }
            HeaderLabel.Text += $"\nInformation has been written to \"Logs\\Exceptions\\{exceptionFilename}.txt\".";
        }
        catch (Exception e)
        {
            HeaderLabel.Text += $"\nCouldn't write error log: {e.Message}.";
        }
    }

    private void CloseButton_OnClick(object? Sender, RoutedEventArgs E)
    {
        this.Close();
    }

    private void CopyErrorButton_OnClick(object? Sender, RoutedEventArgs E)
    {
        ClipboardService.SetText(LogText);
    }
}