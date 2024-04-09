using Avalonia.Controls;
using DBuddy.AppUi.ViewModels;

namespace DBuddy.AppUi.Views;

public partial class HomePage : UserControl
{
    public HomePage()
    {
        InitializeComponent();
        this.DataContext = new HomePageViewModel();
    }
}