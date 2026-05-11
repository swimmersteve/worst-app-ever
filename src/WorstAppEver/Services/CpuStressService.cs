using System.Diagnostics;

namespace WorstAppEver.Services;

public sealed class CpuStressService : IDisposable
{
    private CancellationTokenSource? _cts;
    private readonly List<Thread> _threads = new();

    public bool IsRunning => _cts is { IsCancellationRequested: false };

    public void Start(int threadCount, int targetPercent)
    {
        Stop();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;
        for (int i = 0; i < threadCount; i++)
        {
            var t = new Thread(() => BusyLoop(targetPercent, token))
            {
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal
            };
            t.Start();
            _threads.Add(t);
        }
    }

    public void Stop()
    {
        _cts?.Cancel();
        _cts = null;
        _threads.Clear();
    }

    private static void BusyLoop(int percent, CancellationToken token)
    {
        var sw = Stopwatch.StartNew();
        while (!token.IsCancellationRequested)
        {
            if (sw.ElapsedMilliseconds < percent) continue;
            Thread.Sleep(100 - percent);
            sw.Restart();
        }
    }

    public void Dispose() => Stop();
}
