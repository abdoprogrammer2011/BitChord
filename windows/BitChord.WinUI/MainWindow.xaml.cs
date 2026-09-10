using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinRT.Interop;

namespace BitChord.WinUI;

public sealed partial class MainWindow : Window
{
    private bool _isPlaying = true;

    public MainWindow()
    {
        InitializeComponent();
        RootNavigation.SelectedItem = RootNavigation.MenuItems[0];
        RootNavigation.Header = null;

        var handle = WindowNative.GetWindowHandle(this);
        var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
        var appWindow = AppWindow.GetFromWindowId(windowId);
        appWindow.TitleBar.ExtendsContentIntoTitleBar = true;
        appWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
        appWindow.Resize(new Windows.Graphics.SizeInt32(1440, 900));

        ContentFrame.Navigate(typeof(Pages.HomePage));
    }

    private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is not NavigationViewItem item) return;
        var pageType = item.Tag?.ToString() switch
        {
            "explore" => typeof(Pages.ExplorePage),
            "library" => typeof(Pages.LibraryPage),
            "downloads" => typeof(Pages.DownloadsPage),
            "queue" => typeof(Pages.QueuePage),
            "settings" => typeof(Pages.SettingsPage),
            _ => typeof(Pages.HomePage)
        };
        ContentFrame.Navigate(pageType);
    }

    private void PlayButton_Click(object sender, RoutedEventArgs e)
    {
        _isPlaying = !_isPlaying;
        PlayButton.Content = _isPlaying ? "Ⅱ" : "▶";
    }

    private void PreviousButton_Click(object sender, RoutedEventArgs e) { }
    private void NextButton_Click(object sender, RoutedEventArgs e) { }
}
