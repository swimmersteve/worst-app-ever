namespace WorstAppEver.Services;

public static class HangService
{
    public static void SleepForever()
        => Thread.Sleep(Timeout.Infinite);

    public static void SpinForever()
    {
        while (true) { /* intentional CPU spin on UI thread */ }
    }

    public static void Deadlock()
    {
        object lockA = new();
        object lockB = new();

        var t = new Thread(() =>
        {
            lock (lockB)
            {
                Thread.Sleep(50);
                lock (lockA) { }
            }
        }) { IsBackground = true };
        t.Start();

        lock (lockA)
        {
            Thread.Sleep(100);
            lock (lockB) { } // UI thread blocks here — deadlock
        }
    }

    public static void WaitOnNeverCompletingTask()
    {
        var tcs = new TaskCompletionSource();
        tcs.Task.GetAwaiter().GetResult(); // blocks calling thread forever
    }
}
