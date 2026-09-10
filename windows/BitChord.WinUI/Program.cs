using Microsoft.UI.Xaml;

namespace BitChord.WinUI;

public static class Program
{
    [global::System.STAThread]
    public static void Main(string[] args)
    {
        Application.Start((ApplicationInitializationCallbackParams p) => new App());
    }
}
