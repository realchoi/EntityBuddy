using EntityBuddy.Services;
using ReactiveUI;

namespace EntityBuddy.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _contentViewModel;

        // 这个视图模型依赖于 ToDoListService

        public MainWindowViewModel()
        {
            var service = new ToDoListService();
            ToDoList = new ToDoListViewModel(service.GetItems());
            _contentViewModel = ToDoList;
        }

        public ToDoListViewModel ToDoList { get; }

        public ViewModelBase ContentViewModel
        {
            get => _contentViewModel;
            private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
        }

        public void AddItem()
        {
            ContentViewModel = new AddItemViewModel();
        }
    }
}
