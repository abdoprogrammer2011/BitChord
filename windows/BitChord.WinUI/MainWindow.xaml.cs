using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.Graphics;
using WinRT.Interop;

namespace BitChord.WinUI;

public sealed class MainWindow : Window
{
    private readonly Frame _contentFrame = new();
    private readonly Button _playButton = new() { Content = "▶", FontSize = 17 };
    private bool _isPlaying = true;

    public MainWindow()
    {
        Title = "BitChord";
        ConfigureWindow();

        var root = new Grid { Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent) };
        root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var navigation = new NavigationView
        {
            IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed,
            IsSettingsVisible = false,
            PaneDisplayMode = NavigationViewPaneDisplayMode.Left,
            Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent),
            PaneBackground = new SolidColorBrush(Microsoft.UI.Colors.Transparent),
            Header = null
        };
        navigation.MenuItems.Add(NavigationItem("Home", Symbol.Home, "home"));
        navigation.MenuItems.Add(NavigationItem("Explore", Symbol.World, "explore"));
        navigation.MenuItems.Add(NavigationItem("Library", Symbol.Library, "library"));
        navigation.MenuItems.Add(NavigationItem("Downloads", Symbol.Download, "downloads"));
        navigation.MenuItems.Add(NavigationItem("Queue", Symbol.List, "queue"));
        navigation.FooterMenuItems.Add(NavigationItem("Settings", Symbol.Setting, "settings"));
        navigation.SelectionChanged += Navigation_SelectionChanged;

        var brand = new StackPanel { Margin = new Thickness(20, 20, 20, 28) };
        brand.Children.Add(new TextBlock { Text = "B  BitChord", FontSize = 20, FontWeight = Windows.UI.Text.FontWeights.SemiBold });
        brand.Children.Add(new TextBlock { Text = "Music, in full color", FontSize = 11, Foreground = SecondaryBrush() });
        navigation.PaneHeader = brand;
        navigation.Content = _contentFrame;
        Grid.SetRow(navigation, 0);
        root.Children.Add(navigation);

        var player = CreatePlayerBar();
        Grid.SetRow(player, 1);
        root.Children.Add(player);
        Content = root;

        navigation.SelectedItem = navigation.MenuItems[0];
        ShowPage("home");
    }

    private void ConfigureWindow()
    {
        var handle = WindowNative.GetWindowHandle(this);
        var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
        var appWindow = AppWindow.GetFromWindowId(id);
        appWindow.TitleBar.ExtendsContentIntoTitleBar = true;
        appWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
        appWindow.Resize(new SizeInt32(1440, 900));
    }

    private static NavigationViewItem NavigationItem(string label, Symbol icon, string tag) =>
        new() { Content = label, Tag = tag, Icon = new SymbolIcon { Symbol = icon } };

    private void Navigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is NavigationViewItem item) ShowPage(item.Tag?.ToString() ?? "home");
    }

    private void ShowPage(string tag)
    {
        var title = tag switch
        {
            "explore" => "Explore",
            "library" => "Your library",
            "downloads" => "Downloads",
            "queue" => "Up next",
            "settings" => "Settings",
            _ => "Good evening, Alex"
        };
        var subtitle = tag switch
        {
            "explore" => "Find your next favorite sound.",
            "library" => "Everything you saved, ready offline.",
            "downloads" => "Your music, even when the signal disappears.",
            "queue" => "4 tracks in your queue.",
            "settings" => "Tune BitChord to your setup.",
            _ => "Find your next favorite sound."
        };
        var page = new StackPanel { Spacing = 16, Margin = new Thickness(44, 38, 44, 120), MaxWidth = 1100 };
        page.Children.Add(new TextBlock { Text = title, FontSize = 34, FontWeight = Windows.UI.Text.FontWeights.SemiBold });
        page.Children.Add(new TextBlock { Text = subtitle, FontSize = 16, Foreground = SecondaryBrush() });
        page.Children.Add(new Border
        {
            Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(66, 181, 156, 255)),
            CornerRadius = new CornerRadius(22),
            Padding = new Thickness(24),
            Child = new StackPanel
            {
                Spacing = 12,
                Children =
                {
                    new TextBlock { Text = tag == "settings" ? "Liquid glass" : "BitChord", FontSize = 24, FontWeight = Windows.UI.Text.FontWeights.SemiBold },
                    new TextBlock { Text = tag == "settings" ? "Translucent surfaces, artwork colors, and reduced motion." : "A polished native Windows music experience.", Foreground = SecondaryBrush() },
                    new Button { Content = tag == "settings" ? "Enabled" : "Play", HorizontalAlignment = HorizontalAlignment.Left }
                }
            }
        });
        _contentFrame.Content = new ScrollViewer { Content = page };
    }

    private UIElement CreatePlayerBar()
    {
        var bar = new Border
        {
            Margin = new Thickness(24, 0, 24, 20),
            Padding = new Thickness(16, 12, 16, 12),
            Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(66, 255, 255, 255)),
            CornerRadius = new CornerRadius(22)
        };
        var row = new StackPanel { Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Center };
        row.Children.Add(new TextBlock { Text = "♫", FontSize = 30, Margin = new Thickness(0, 0, 14, 0) });
        var track = new StackPanel { Width = 250 };
        track.Children.Add(new TextBlock { Text = "Midnight City", FontWeight = Windows.UI.Text.FontWeights.SemiBold });
        track.Children.Add(new TextBlock { Text = "M83  •  Hurry Up, We're Dreaming", FontSize = 12, Foreground = SecondaryBrush() });
        row.Children.Add(track);
        var previous = new Button { Content = "‹", FontSize = 24 };
        previous.Click += (_, _) => { };
        row.Children.Add(previous);
        _playButton.Click += PlayButton_Click;
        row.Children.Add(_playButton);
        var next = new Button { Content = "›", FontSize = 24 };
        next.Click += (_, _) => { };
        row.Children.Add(next);
        row.Children.Add(new Slider { Width = 160, Value = 42, Margin = new Thickness(18, 0, 0, 0) });
        bar.Child = row;
        return bar;
    }

    private void PlayButton_Click(object sender, RoutedEventArgs e)
    {
        _isPlaying = !_isPlaying;
        _playButton.Content = _isPlaying ? "Ⅱ" : "▶";
    }

    private static SolidColorBrush SecondaryBrush() =>
        new(Microsoft.UI.ColorHelper.FromArgb(255, 169, 170, 188));
}
