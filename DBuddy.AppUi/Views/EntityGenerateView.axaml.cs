using Avalonia.Controls;
using DBuddy.AppUi.ViewModels;

namespace DBuddy.AppUi.Views;

public partial class EntityGenerateView : UserControl
{
    public EntityGenerateView()
    {
        InitializeComponent();
        this.DataContext = new EntityGenerateViewModel();
    }
}