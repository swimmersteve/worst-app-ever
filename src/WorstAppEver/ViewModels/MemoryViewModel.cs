using System.Windows.Input;
using WorstAppEver.Services;

namespace WorstAppEver.ViewModels;

public sealed class MemoryViewModel : ViewModelBase, IDisposable
{
    private readonly MemoryStressService _service;
    private int _targetMegabytes = 1024;
    private bool _isAllocated;
    private string _allocatedDisplay = "None";

    public int TargetMegabytes
    {
        get => _targetMegabytes;
        set => Set(ref _targetMegabytes, Math.Max(256, value));
    }

    public bool IsAllocated
    {
        get => _isAllocated;
        private set => Set(ref _isAllocated, value);
    }

    public string AllocatedDisplay
    {
        get => _allocatedDisplay;
        private set => Set(ref _allocatedDisplay, value);
    }

    public ICommand AllocateCommand { get; }
    public ICommand ReleaseCommand  { get; }

    public MemoryViewModel(MemoryStressService service)
    {
        _service = service;
        AllocateCommand = new RelayCommand(Allocate, () => !IsAllocated);
        ReleaseCommand  = new RelayCommand(Release,  () => IsAllocated);
    }

    private void Allocate()
    {
        Task.Run(() =>
        {
            _service.Allocate(TargetMegabytes);
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                IsAllocated = true;
                AllocatedDisplay = _service.AllocatedDisplay;
                ((RelayCommand)AllocateCommand).RaiseCanExecuteChanged();
                ((RelayCommand)ReleaseCommand).RaiseCanExecuteChanged();
            });
        });
    }

    private void Release()
    {
        _service.Release();
        IsAllocated = false;
        AllocatedDisplay = "None";
        ((RelayCommand)AllocateCommand).RaiseCanExecuteChanged();
        ((RelayCommand)ReleaseCommand).RaiseCanExecuteChanged();
    }

    public void Dispose() => _service.Dispose();
}
