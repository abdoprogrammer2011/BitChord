using Microsoft.UI.Xaml;
using System;
using System.IO;

namespace BitChord.WinUI;

public static class Program
{
    private static readonly object LogLock = new();

    [global::System.STAThread]
    public static void Main(string[] args)
    {
        AppDomain.CurrentDomain.UnhandledException += (_, eventArgs) =>
            StartupDiagnostics.Write("AppDomain.CurrentDomain.UnhandledException", eventArgs.ExceptionObject);
        TaskScheduler.UnobservedTaskException += (_, eventArgs) =>
        {
            StartupDiagnostics.Write("TaskScheduler.UnobservedTaskException", eventArgs.Exception);
            eventArgs.SetObserved();
        };

        try
        {
            Application.Start((ApplicationInitializationCallbackParams p) => new App());
        }
        catch (Exception exception)
        {
            StartupDiagnostics.Write("Application.Start", exception);
            throw;
        }
    }

    internal static class StartupDiagnostics
    {
        private static string LogPath
        {
            get
            {
                var desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                return Path.Combine(
                    string.IsNullOrWhiteSpace(desktop)
                        ? Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                        : desktop,
                    "BitChord-startup.log");
            }
        }

        public static void Write(string source, object exception)
        {
            try
            {
                lock (LogLock)
                {
                    File.AppendAllText(
                        LogPath,
                        $"{DateTimeOffset.Now:O} [{source}]{Environment.NewLine}{exception}{Environment.NewLine}{Environment.NewLine}");
                }
            }
            catch
            {
                // Diagnostics must never become another startup failure.
            }
        }
    }
}
