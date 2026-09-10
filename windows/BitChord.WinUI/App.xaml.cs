using Microsoft.UI.Xaml;

namespace BitChord.WinUI;

public sealed class App : Application
{
    public static Window? MainWindow { get; private set; }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        MainWindow = new MainWindow();
        MainWindow.Activate();
    }
}
