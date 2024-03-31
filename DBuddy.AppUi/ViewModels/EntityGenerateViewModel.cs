using AvaloniaEdit.Utils;
using DBuddy.Model.Enums;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using DBuddy.Model;
using SpringMountain.Infrastructure.Tools;

namespace DBuddy.AppUi.ViewModels;

public class EntityGenerateViewModel : ViewModelBase
{
    public EntityGenerateViewModel()
    {
        DatabaseTypes.AddRange(EnumTool.GetAllMembers<DatabaseType>().Where(item => item.Value != 0)
            .Select(item => new ComboBoxItemDto<int>((int)item.Value, item.Description)));
        ProgrammingLanguages.AddRange(EnumTool.GetAllMembers<ProgrammingLanguage>().Where(item => item.Value != 0)
            .Select(item => new ComboBoxItemDto<int>((int)item.Value, item.Description)));

        _saveClassFilePath = string.Empty;
        SelectSaveClassFilePathCommand = ReactiveCommand.Create(SelectSaveClassFilePath);
    }

    /// <summary>
    /// 数据库类型下拉选项
    /// </summary>
    public ObservableCollection<ComboBoxItemDto<int>> DatabaseTypes { get; } = [];

    /// <summary>
    /// 编程语言下拉选项
    /// </summary>
    public ObservableCollection<ComboBoxItemDto<int>> ProgrammingLanguages { get; } = [];

    #region Class 文件保存路径

    private string _saveClassFilePath;

    public string SaveClassFilePath
    {
        get => _saveClassFilePath;
        set => this.RaiseAndSetIfChanged(ref _saveClassFilePath, value);
    }

    #endregion


    #region Command

    public ReactiveCommand<Unit, Unit> SelectSaveClassFilePathCommand { get; }

    #endregion

    private async void SelectSaveClassFilePath()
    {
        // var dialog = new OpenFolderDialog
        // {
        //     Title = "选择保存路径",
        //     Directory = Environment.CurrentDirectory
        // };
        // var result = await dialog.ShowAsync(Views.MainWindow.Instance);
        // if (result != null)
        // {
        //     SaveClassFilePath = result;
        // }
        var dialog = new FolderPickerOpenOptions
        {
            Title = "选择保存路径",
            AllowMultiple = false
        };
        var result =
            await (Application.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!.MainWindow!
                .StorageProvider.OpenFolderPickerAsync(dialog);
        if (result is { Count: > 0 })
        {
            SaveClassFilePath = result[0].Path.AbsolutePath;
        }
    }
}