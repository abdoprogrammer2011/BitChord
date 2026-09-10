using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using System.Threading;

namespace BitChord.WinUI;

public static class Program
{
    [global::System.STAThread]
    public static void Main(string[] args)
    {
        Application.Start((DispatcherQueue dispatcherQueue) =>
        {
            var context = new DispatcherQueueSynchronizationContext(dispatcherQueue);
            SynchronizationContext.SetSynchronizationContext(context);
            _ = new App();
        });
    }
}
