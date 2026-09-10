using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;

namespace BitChord.WinUI;

public sealed partial class App : Application
{
    public static Window? MainWindow { get; private set; }

    public App()
    {
        InitializeComponent();

        if (!Resources.ContainsKey("TabViewButtonBackground"))
        {
            Resources["TabViewButtonBackground"] =
                new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        }

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
