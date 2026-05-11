using System.Security.Principal;
using System.Windows.Input;
using WorstAppEver.Services;

namespace WorstAppEver.ViewModels;

public sealed class BsodViewModel : ViewModelBase
{
    private string _confirmText = string.Empty;

    public bool IsAdmin { get; }

    public string ConfirmText
    {
        get => _confirmText;
        set
        {
            Set(ref _confirmText, value);
            ((RelayCommand)TriggerBsodCommand).RaiseCanExecuteChanged();
        }
    }

    public bool CanTrigger => IsAdmin && ConfirmText.Trim().Equals("BSOD", StringComparison.OrdinalIgnoreCase);

    public ICommand TriggerBsodCommand { get; }

    public BsodViewModel()
    {
        using var identity = WindowsIdentity.GetCurrent();
        IsAdmin = new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
        TriggerBsodCommand = new RelayCommand(BsodService.TriggerBsod, () => CanTrigger);
    }
}
