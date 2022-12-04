using Avalonia;
using System;
using System.Diagnostics;
using CBRE.Extended.Editor.Logging;

namespace CBRE.Extended.Editor
{
    public class EntryPoint
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
        
        private static void LogException(Exception Exception)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame[] stackFrames = stackTrace.GetFrames();
            string message = "Unhandled exception";
            
            foreach (StackFrame frame in stackFrames)
            {
                System.Reflection.MethodBase method = frame.GetMethod();
                message += "\r\n    " + method.ReflectedType.FullName + "." + method.Name;
            }
            
            Logger.ShowException(new Exception(message, Exception), "Unhandled exception");
        }
    }
}