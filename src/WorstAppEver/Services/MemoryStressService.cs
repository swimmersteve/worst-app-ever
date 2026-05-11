namespace WorstAppEver.Services;

public sealed class MemoryStressService : IDisposable
{
    private readonly List<byte[]> _held = new();

    public long AllocatedBytes { get; private set; }
    public bool IsRunning => _held.Count > 0;

    public string AllocatedDisplay =>
        AllocatedBytes == 0 ? "None" :
        AllocatedBytes >= 1024L * 1024 * 1024
            ? $"{AllocatedBytes / (1024.0 * 1024 * 1024):F2} GB held"
            : $"{AllocatedBytes / (1024.0 * 1024):F0} MB held";

    public void Allocate(int megabytes)
    {
        Release();
        const int chunkBytes = 256 * 1024 * 1024; // 256 MB per chunk
        long remaining = (long)megabytes * 1024 * 1024;
        while (remaining > 0)
        {
            int chunk = (int)Math.Min(remaining, chunkBytes);
            var arr = new byte[chunk];
            for (int i = 0; i < arr.Length; i += 4096) arr[i] = 1;
            _held.Add(arr);
            remaining -= chunk;
        }
        AllocatedBytes = (long)megabytes * 1024 * 1024;
    }

    public void Release()
    {
        _held.Clear();
        AllocatedBytes = 0;
        GC.Collect(2, GCCollectionMode.Forced, blocking: true);
    }

    public void Dispose() => Release();
}
