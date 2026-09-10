using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using System.Threading;

namespace BitChord.WinUI;

public static class Program
{
    [global::System.STAThread]
    public static void Main(string[] args)
    {
        Application.Start(_ =>
        {
            var context = new DispatcherQueueSynchronizationContext(
                DispatcherQueue.GetForCurrentThread());
            SynchronizationContext.SetSynchronizationContext(context);
            _ = new App();
        });
    }
}
