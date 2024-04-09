using System.Windows.Input;
using ReactiveUI;

namespace DBuddy.AppUi.ViewModels;

public class HomePageViewModel : ViewModelBase
{
    public HomePageViewModel()
    {
        OpenEntityGenerateCommand = ReactiveCommand.Create(OpenEntityGenerateView);
    }
    
    public ICommand OpenEntityGenerateCommand { get; }

    private void OpenEntityGenerateView()
    {
        // // 创建 EntityGenerateView 实例
        // var entityGenerateView = new Views.EntityGenerateView();
        //
        // // 创建新窗口并显示 MarkdownEditView
        // var window = new Window
        // {
        //     Content = entityGenerateView,
        //     DataContext = new EntityGenerateViewModel()
        // };
        //
        // window.Show();

        // 打开生成实体的弹窗
        VmLocator.MainWindowViewModel.EntityGenerateViewIsOpened = true;
    }
}