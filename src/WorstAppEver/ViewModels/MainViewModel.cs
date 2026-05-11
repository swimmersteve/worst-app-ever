using WorstAppEver.Services;

namespace WorstAppEver.ViewModels;

public sealed class MainViewModel : ViewModelBase, IDisposable
{
    public CrashViewModel  Crash  { get; }
    public HangViewModel   Hang   { get; }
    public CpuViewModel    Cpu    { get; }
    public MemoryViewModel Memory { get; }
    public DiskViewModel   Disk   { get; }
    public BsodViewModel   Bsod   { get; }

    public MainViewModel(
        CpuStressService    cpuService,
        MemoryStressService memoryService,
        DiskStressService   diskService)
    {
        Crash  = new CrashViewModel();
        Hang   = new HangViewModel();
        Cpu    = new CpuViewModel(cpuService);
        Memory = new MemoryViewModel(memoryService);
        Disk   = new DiskViewModel(diskService);
        Bsod   = new BsodViewModel();
    }

    public void Dispose()
    {
        Cpu.Dispose();
        Memory.Dispose();
        Disk.Dispose();
    }
}
