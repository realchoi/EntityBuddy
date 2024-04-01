using System;
using AvaloniaEdit.Utils;
using DBuddy.Model.Enums;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Web;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using DBuddy.Model;
using DBuddy.Service.Infrastructures.Utils;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using SpringMountain.Infrastructure.Tools;

namespace DBuddy.AppUi.ViewModels;

public class EntityGenerateViewModel : ViewModelBase
{
    public EntityGenerateViewModel()
    {
        InitComboBoxSource();

        _selectedProgrammingLanguage = new ComboBoxItemDto<int>(-1, "请选择编程语言");
        _selectedDatabaseType = new ComboBoxItemDto<int>(-1, "请选择数据库类型");
        _connectionString = string.Empty;
        _saveClassFilePath = string.Empty;

        // 初始化事件命令
        TestDbConnectCommand = ReactiveCommand.Create(TestDbConnect);
        SelectSaveClassFilePathCommand = ReactiveCommand.Create(SelectSaveClassFilePath);
        GenerateClassFileCommand = ReactiveCommand.Create(GenerateClassFile);
    }

    /// <summary>
    /// 主窗口
    /// </summary>
    private static Window MainWindow =>
        (Application.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!.MainWindow!;

    #region 下拉选项数据源

    /// <summary>
    /// 数据库类型下拉选项
    /// </summary>
    public ObservableCollection<ComboBoxItemDto<int>> DatabaseTypes { get; } = [];

    /// <summary>
    /// 编程语言下拉选项
    /// </summary>
    public ObservableCollection<ComboBoxItemDto<int>> ProgrammingLanguages { get; } = [];

    #endregion

    #region 双向绑定的属性

    /// <summary>
    /// 填写的数据库连接字符串
    /// </summary>
    private string _connectionString;

    /// <summary>
    /// 填写的数据库连接字符串
    /// </summary>
    public string ConnectionString
    {
        get => _connectionString;
        set => this.RaiseAndSetIfChanged(ref _connectionString, value);
    }

    /// <summary>
    /// 选择的编程语言
    /// </summary>
    private ComboBoxItemDto<int> _selectedProgrammingLanguage;

    /// <summary>
    /// 选择的编程语言
    /// </summary>
    public ComboBoxItemDto<int> SelectedProgrammingLanguage
    {
        get => _selectedProgrammingLanguage;
        set => this.RaiseAndSetIfChanged(ref _selectedProgrammingLanguage, value);
    }

    /// <summary>
    /// 选择的数据库类型
    /// </summary>
    private ComboBoxItemDto<int> _selectedDatabaseType;

    /// <summary>
    /// 选择的数据库类型
    /// </summary>
    public ComboBoxItemDto<int> SelectedDatabaseType
    {
        get => _selectedDatabaseType;
        set => this.RaiseAndSetIfChanged(ref _selectedDatabaseType, value);
    }

    /// <summary>
    /// Class 文件保存路径
    /// </summary>
    private string _saveClassFilePath;

    /// <summary>
    /// Class 文件保存路径
    /// </summary>
    public string SaveClassFilePath
    {
        get => _saveClassFilePath;
        set => this.RaiseAndSetIfChanged(ref _saveClassFilePath, value);
    }

    #endregion

    #region 命令事件 Command

    /// <summary>
    /// 选择保存路径 Command
    /// </summary>
    public ReactiveCommand<Unit, Unit> SelectSaveClassFilePathCommand { get; }

    /// <summary>
    /// 测试数据库连接 Command
    /// </summary>
    public ReactiveCommand<Unit, Unit> TestDbConnectCommand { get; }

    /// <summary>
    /// 生成 Class 文件 Command
    /// </summary>
    public ReactiveCommand<Unit, Unit> GenerateClassFileCommand { get; }

    #endregion

    #region 私有方法

    /// <summary>
    /// 测试数据库连接
    /// </summary>
    private async void TestDbConnect()
    {
        if (ConnectionString.IsNullOrWhiteSpace())
        {
            var infoBox = MessageBoxManager.GetMessageBoxStandard("提示", "连接字符串不能为空！", ButtonEnum.Ok, Icon.Warning);
            await infoBox.ShowAsync();
            return;
        }

        var databaseTypeEnum = (DatabaseType)SelectedDatabaseType!.Value;
        switch (databaseTypeEnum)
        {
            case DatabaseType.PostgreSql:
                var errorMsg = await DbHelper.TryConnectPostgreSqlAsync(ConnectionString!);
                if (errorMsg != null)
                {
                    var errorBox = MessageBoxManager.GetMessageBoxStandard("提示", $"连接失败！\r\n{errorMsg}",
                        ButtonEnum.Ok, Icon.Error);
                    await errorBox.ShowAsync();
                }
                else
                {
                    var successBox = MessageBoxManager.GetMessageBoxStandard("提示", "连接成功！\r\n现在可以生成实体了。",
                        ButtonEnum.Ok, Icon.Success);
                    await successBox.ShowAsync();
                }

                break;
            case DatabaseType.Unknown:
            case DatabaseType.Oracle:
            case DatabaseType.MySql:
            case DatabaseType.SqlServer:
            default:
                break;
        }
    }


    /// <summary>
    /// 生成 Class 文件
    /// </summary>
    private async void GenerateClassFile()
    {
        if (ConnectionString.IsNullOrWhiteSpace())
        {
            var infoBox = MessageBoxManager.GetMessageBoxStandard("提示", "连接字符串不能为空！", ButtonEnum.Ok, Icon.Warning);
            await infoBox.ShowAsync();
            return;
        }

        var databaseTypeEnum = (DatabaseType)SelectedDatabaseType!.Value;
        switch (databaseTypeEnum)
        {
            case DatabaseType.PostgreSql:
                var successBox = MessageBoxManager.GetMessageBoxStandard("提示", "Class 文件生成成功！",
                    ButtonEnum.Ok, Icon.Success);
                await successBox.ShowAsync();
                break;
            case DatabaseType.Unknown:
            case DatabaseType.Oracle:
            case DatabaseType.MySql:
            case DatabaseType.SqlServer:
            default:
                break;
        }
    }


    /// <summary>
    /// 选择 Class 文件保存路径
    /// </summary>
    private async void SelectSaveClassFilePath()
    {
        var suggestedPath = _saveClassFilePath.IsNullOrWhiteSpace()
            ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            : _saveClassFilePath;
        var dialog = new FolderPickerOpenOptions
        {
            Title = "选择保存路径",
            AllowMultiple = false,
            SuggestedStartLocation = await MainWindow.StorageProvider.TryGetFolderFromPathAsync(new Uri(suggestedPath))
        };
        var result = await MainWindow.StorageProvider.OpenFolderPickerAsync(dialog);
        if (result is { Count: > 0 })
        {
            SaveClassFilePath = HttpUtility.UrlDecode(result[0].Path.AbsolutePath);
        }
    }


    /// <summary>
    /// 初始化下拉选项数据源
    /// </summary>
    private void InitComboBoxSource()
    {
        var databaseTypes = EnumTool.GetAllMembers<DatabaseType>()
            .Where(item => item.Value != 0)
            .Select(item => new ComboBoxItemDto<int>((int)item.Value, item.Description));
        DatabaseTypes.AddRange(databaseTypes);

        var programmingLanguages = EnumTool.GetAllMembers<ProgrammingLanguage>()
            .Where(item => item.Value != 0)
            .Select(item => new ComboBoxItemDto<int>((int)item.Value, item.Description));
        ProgrammingLanguages.AddRange(programmingLanguages);
    }

    #endregion
}