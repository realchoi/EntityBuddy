using Avalonia.Controls;
using ReactiveUI;
using System.Windows.Input;

namespace DBuddy.AppUi.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _contentViewModel;

    public MainWindowViewModel()
    {
        OpenEntityGenerateCommand = ReactiveCommand.Create(OpenEntityGenerateView);
    }

    public ViewModelBase ContentViewModel
    {
        get => _contentViewModel;
        private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
    }

    public ICommand OpenEntityGenerateCommand { get; }

    private void OpenEntityGenerateView()
    {
        // 创建 EntityGenerateView 实例
        var entityGenerateView = new Views.EntityGenerateView();

        // 创建新窗口并显示 MarkdownEditView
        var window = new Window
        {
            Content = entityGenerateView,
            DataContext = new EntityGenerateViewModel()
        };

        window.Show();
    }
}