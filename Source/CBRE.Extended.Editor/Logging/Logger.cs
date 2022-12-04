using System;
using CBRE.Extended.Editor.Logging.UI;

namespace CBRE.Extended.Editor.Logging;

public static class Logger
{
    public static void ShowException(Exception Exception, string Message = "")
    {
        ExceptionInfo exceptionInfo = new ExceptionInfo(Exception, Message);
        ExceptionWindow exceptionWindow = new ExceptionWindow();
        
        exceptionWindow.SetUp(exceptionInfo);
        
        if(Editor.Instance == null || !Editor.Instance.IsVisible) exceptionWindow.Show();
        else exceptionWindow.Show(Editor.Instance);
    }
}