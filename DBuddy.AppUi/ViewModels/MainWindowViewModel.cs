using ReactiveUI;

namespace DBuddy.AppUi.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _contentViewModel;

        // 这个视图模型依赖于 ToDoListService

        public MainWindowViewModel()
        {
        }

        public ViewModelBase ContentViewModel
        {
            get => _contentViewModel;
            private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
        }
    }
}
