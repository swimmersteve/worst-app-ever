using System.Windows.Input;
using WorstAppEver.Services;

namespace WorstAppEver.ViewModels;

public sealed class HangViewModel : ViewModelBase
{
    public ICommand SleepForeverCommand          { get; }
    public ICommand SpinForeverCommand           { get; }
    public ICommand DeadlockCommand              { get; }
    public ICommand TaskNeverCompletesCommand    { get; }

    public HangViewModel()
    {
        SleepForeverCommand       = new RelayCommand(HangService.SleepForever);
        SpinForeverCommand        = new RelayCommand(HangService.SpinForever);
        DeadlockCommand           = new RelayCommand(HangService.Deadlock);
        TaskNeverCompletesCommand = new RelayCommand(HangService.WaitOnNeverCompletingTask);
    }
}
