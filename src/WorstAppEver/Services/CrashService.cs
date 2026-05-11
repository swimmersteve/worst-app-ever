using System.Runtime.InteropServices;

namespace WorstAppEver.Services;

public static class CrashService
{
    public static void AccessViolation()
        => Marshal.ReadInt32(IntPtr.Zero);

    public static void StackOverflow() => StackOverflow();

    public static void UnhandledException()
        => throw new InvalidOperationException("WorstAppEver: deliberate unhandled exception — this crash was intentional.");

    public static void DivisionByZero()
    {
        int x = 0;
        _ = 1 / x;
    }

    public static void FailFast()
        => Environment.FailFast("WorstAppEver: deliberate FailFast — this termination was intentional.");

    public static void OutOfMemory()
    {
        var sink = new List<byte[]>();
        while (true)
        {
            var chunk = new byte[128 * 1024 * 1024];
            // Touch every page to commit physical RAM
            for (int i = 0; i < chunk.Length; i += 4096) chunk[i] = 1;
            sink.Add(chunk);
        }
    }
}
