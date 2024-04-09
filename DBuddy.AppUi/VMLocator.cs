using DBuddy.AppUi.ViewModels;

namespace DBuddy.AppUi;

public class VmLocator
{
    private static MainWindowViewModel _mainWindowViewModel;

    public static MainWindowViewModel MainWindowViewModel
    {
        get => _mainWindowViewModel ??= new MainWindowViewModel();
        set => _mainWindowViewModel = value;
    }
}