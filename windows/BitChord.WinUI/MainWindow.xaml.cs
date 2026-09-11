using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Text;
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

        var root = new Grid { Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent) };
        root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var navigation = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(220) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            }
        };

        var brand = new StackPanel { Margin = new Thickness(20, 20, 20, 28) };
        brand.Children.Add(new TextBlock { Text = "B  BitChord", FontSize = 20, FontWeight = FontWeights.SemiBold });
        brand.Children.Add(new TextBlock { Text = "Music, in full color", FontSize = 11, Foreground = SecondaryBrush() });
        var menu = new StackPanel();
        menu.Children.Add(brand);
        foreach (var item in new[] { ("Home", "home"), ("Explore", "explore"), ("Library", "library"), ("Downloads", "downloads"), ("Queue", "queue"), ("Settings", "settings") })
        {
            var button = new Button
            {
                Content = item.Item1,
                Tag = item.Item2,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(12, 2, 12, 2),
                Padding = new Thickness(12, 10, 12, 10),
                Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent),
                Foreground = new SolidColorBrush(Microsoft.UI.Colors.White),
                BorderThickness = new Thickness(0)
            };
            button.Click += NavigationButton_Click;
            menu.Children.Add(button);
        }

        Grid.SetColumn(menu, 0);
        navigation.Children.Add(menu);
        Grid.SetColumn(_contentFrame, 1);
        navigation.Children.Add(_contentFrame);
        Grid.SetRow(navigation, 0);
        root.Children.Add(navigation);

        var player = CreatePlayerBar();
        Grid.SetRow(player, 1);
        root.Children.Add(player);
        Content = root;

        ConfigureWindow();
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

    private void NavigationButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button) ShowPage(button.Tag?.ToString() ?? "home");
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
        page.Children.Add(new TextBlock { Text = title, FontSize = 34, FontWeight = FontWeights.SemiBold });
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
                    new TextBlock { Text = tag == "settings" ? "Liquid glass" : "BitChord", FontSize = 24, FontWeight = FontWeights.SemiBold },
                    new TextBlock { Text = tag == "settings" ? "Translucent surfaces, artwork colors, and reduced motion." : "A polished native Windows music experience.", Foreground = SecondaryBrush() },
                    new Button { Content = tag == "settings" ? "Enabled" : "Play", HorizontalAlignment = HorizontalAlignment.Left }
                }
            }
        });
        _contentFrame.Content = new ScrollViewer { Content = page };
    }

    private FrameworkElement CreatePlayerBar()
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
        track.Children.Add(new TextBlock { Text = "Midnight City", FontWeight = FontWeights.SemiBold });
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
