using System.Windows.Input;
using WorstAppEver.Services;

namespace WorstAppEver.ViewModels;

public sealed class CpuViewModel : ViewModelBase, IDisposable
{
    private readonly CpuStressService _service;
    private int _targetPercent = 50;
    private int _threadCount;
    private bool _isRunning;

    public int TargetPercent
    {
        get => _targetPercent;
        set { Set(ref _targetPercent, Math.Clamp(value, 1, 99)); OnPropertyChanged(nameof(StatusText)); }
    }

    public int ThreadCount
    {
        get => _threadCount;
        set { Set(ref _threadCount, Math.Max(1, value)); OnPropertyChanged(nameof(StatusText)); }
    }

    public bool IsRunning
    {
        get => _isRunning;
        private set { Set(ref _isRunning, value); OnPropertyChanged(nameof(StatusText)); }
    }

    public string StatusText => IsRunning
        ? $"Running — {ThreadCount} thread(s) at {TargetPercent}% load each"
        : "Idle";

    public ICommand StartCommand { get; }
    public ICommand StopCommand  { get; }

    public CpuViewModel(CpuStressService service)
    {
        _service = service;
        _threadCount = Environment.ProcessorCount;
        StartCommand = new RelayCommand(Start, () => !IsRunning);
        StopCommand  = new RelayCommand(Stop,  () => IsRunning);
    }

    private void Start()
    {
        _service.Start(ThreadCount, TargetPercent);
        IsRunning = true;
        ((RelayCommand)StartCommand).RaiseCanExecuteChanged();
        ((RelayCommand)StopCommand).RaiseCanExecuteChanged();
    }

    private void Stop()
    {
        _service.Stop();
        IsRunning = false;
        ((RelayCommand)StartCommand).RaiseCanExecuteChanged();
        ((RelayCommand)StopCommand).RaiseCanExecuteChanged();
    }

    public void Dispose() => _service.Dispose();
}
