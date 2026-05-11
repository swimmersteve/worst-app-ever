using System.IO;

namespace WorstAppEver.Services;

public sealed class DiskStressService : IDisposable
{
    private CancellationTokenSource? _cts;
    private Task? _task;

    private readonly string _tempPath =
        Path.Combine(Path.GetTempPath(), "WorstAppEver_disk.tmp");

    public bool IsRunning => _task is { IsCompleted: false };

    public void Start(int bufferMb = 64)
    {
        Stop();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;
        _task = Task.Run(() => DiskLoop(bufferMb, token), token);
    }

    public void Stop()
    {
        _cts?.Cancel();
        try { _task?.Wait(2000); } catch { }
        _cts = null;
        _task = null;
        TryDeleteTemp();
    }

    private void DiskLoop(int bufferMb, CancellationToken token)
    {
        var buffer = new byte[bufferMb * 1024 * 1024];
        new Random().NextBytes(buffer);
        while (!token.IsCancellationRequested)
        {
            File.WriteAllBytes(_tempPath, buffer);
            if (token.IsCancellationRequested) break;
            _ = File.ReadAllBytes(_tempPath);
        }
        TryDeleteTemp();
    }

    private void TryDeleteTemp()
    {
        try { if (File.Exists(_tempPath)) File.Delete(_tempPath); }
        catch { }
    }

    public void Dispose() => Stop();
}
