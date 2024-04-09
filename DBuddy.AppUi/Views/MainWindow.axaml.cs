using Avalonia.Controls;
using DBuddy.AppUi.ViewModels;

namespace DBuddy.AppUi.Views;

public partial class MainWindow : Window
{
    public MainWindowViewModel MainWindowViewModel { get; } = new();

    public MainWindow()
    {
        InitializeComponent();

        MainWindowViewModel = new MainWindowViewModel();
        VmLocator.MainWindowViewModel = MainWindowViewModel;
        this.DataContext = MainWindowViewModel;
    }
}