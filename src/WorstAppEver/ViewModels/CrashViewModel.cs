using System.Windows.Input;
using WorstAppEver.Services;

namespace WorstAppEver.ViewModels;

public sealed class CrashViewModel : ViewModelBase
{
    public ICommand AccessViolationCommand { get; }
    public ICommand StackOverflowCommand { get; }
    public ICommand UnhandledExceptionCommand { get; }
    public ICommand DivisionByZeroCommand { get; }
    public ICommand FailFastCommand { get; }
    public ICommand OutOfMemoryCommand { get; }

    public CrashViewModel()
    {
        AccessViolationCommand    = new RelayCommand(CrashService.AccessViolation);
        StackOverflowCommand      = new RelayCommand(CrashService.StackOverflow);
        UnhandledExceptionCommand = new RelayCommand(CrashService.UnhandledException);
        DivisionByZeroCommand     = new RelayCommand(CrashService.DivisionByZero);
        FailFastCommand           = new RelayCommand(CrashService.FailFast);
        OutOfMemoryCommand        = new RelayCommand(() =>
            new Thread(CrashService.OutOfMemory) { IsBackground = false }.Start());
    }
}
