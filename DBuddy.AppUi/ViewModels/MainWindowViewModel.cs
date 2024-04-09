using ReactiveUI;

namespace DBuddy.AppUi.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        EntityGenerateViewIsOpened = false;
    }

    /// <summary>
    /// 生成实体的弹窗是否打开 flag
    /// </summary>
    private bool _entityGenerateViewIsOpened;

    /// <summary>
    /// 生成实体的弹窗是否打开 flag
    /// </summary>
    public bool EntityGenerateViewIsOpened
    {
        get => _entityGenerateViewIsOpened;
        set => this.RaiseAndSetIfChanged(ref _entityGenerateViewIsOpened, value);
    }
}