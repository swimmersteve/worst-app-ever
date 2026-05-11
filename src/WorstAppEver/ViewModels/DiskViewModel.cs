using System.Windows.Input;
using WorstAppEver.Services;

namespace WorstAppEver.ViewModels;

public sealed class DiskViewModel : ViewModelBase, IDisposable
{
    private readonly DiskStressService _service;
    private int _bufferMb = 64;
    private bool _isRunning;

    public int[] BufferSizes { get; } = [4, 16, 64, 256];

    public int BufferMb
    {
        get => _bufferMb;
        set => Set(ref _bufferMb, value);
    }

    public bool IsRunning
    {
        get => _isRunning;
        private set { Set(ref _isRunning, value); OnPropertyChanged(nameof(StatusText)); }
    }

    public string StatusText => IsRunning
        ? $"Running — writing/reading {BufferMb} MB blocks continuously"
        : "Idle";

    public ICommand StartCommand { get; }
    public ICommand StopCommand  { get; }

    public DiskViewModel(DiskStressService service)
    {
        _service = service;
        StartCommand = new RelayCommand(Start, () => !IsRunning);
        StopCommand  = new RelayCommand(Stop,  () => IsRunning);
    }

    private void Start()
    {
        _service.Start(BufferMb);
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
