using Microsoft.UI.Xaml;

namespace BitChord.WinUI;

public sealed class App : Application
{
    public static Window? MainWindow { get; private set; }

    public App()
    {
        UnhandledException += OnUnhandledException;
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        try
        {
            MainWindow = new MainWindow();
            MainWindow.Activate();
        }
        catch (Exception exception)
        {
            Program.StartupDiagnostics.Write("App.OnLaunched", exception);
            throw;
        }
    }

    private static void OnUnhandledException(
        object sender,
        Microsoft.UI.Xaml.UnhandledExceptionEventArgs args)
    {
        Program.StartupDiagnostics.Write("Microsoft.UI.Xaml.Application.UnhandledException", args.Exception);
    }
}
