using System.Windows;
using WorstAppEver.Services;
using WorstAppEver.ViewModels;

namespace WorstAppEver;

public partial class App : Application
{
    private readonly CpuStressService    _cpuService    = new();
    private readonly MemoryStressService _memoryService = new();
    private readonly DiskStressService   _diskService   = new();
    private MainViewModel?               _mainVm;

    private void App_OnStartup(object sender, StartupEventArgs e)
    {
        _mainVm = new MainViewModel(_cpuService, _memoryService, _diskService);
        var window = new MainWindow(_mainVm);
        MainWindow = window;
        window.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _mainVm?.Dispose();
        base.OnExit(e);
    }
}
