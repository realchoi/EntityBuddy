using Avalonia.Controls;

namespace DBuddy.AppUi.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        Instance = this;
        InitializeComponent();
    }

    public static MainWindow Instance;
}