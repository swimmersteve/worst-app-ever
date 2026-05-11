using System.Windows;
using WorstAppEver.ViewModels;

namespace WorstAppEver;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
