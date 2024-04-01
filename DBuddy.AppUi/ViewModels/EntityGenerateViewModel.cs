using System;
using AvaloniaEdit.Utils;
using DBuddy.Model.Enums;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using DBuddy.Model;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Npgsql;
using SpringMountain.Infrastructure.Tools;

namespace DBuddy.AppUi.ViewModels;

public class EntityGenerateViewModel : ViewModelBase
{
    public EntityGenerateViewModel()
    {
        InitComboBoxSource();

        _selectedProgrammingLanguage = new ComboBoxItemDto<int>(-1, "请选择编程语言");
        _selectedDatabaseType = new ComboBoxItemDto<int>(-1, "请选择数据库类型");
        _saveClassFilePath = string.Empty;

        // 初始化事件命令
        TestDbConnectCommand = ReactiveCommand.Create(TestDbConnect);
        SelectSaveClassFilePathCommand = ReactiveCommand.Create(SelectSaveClassFilePath);
    }

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

    #endregion

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
    public ReactiveCommand<Unit, Unit> TestDbConnectCommand { get; }

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
                var errorMsg = await TryConnectPostgreSqlAsync(ConnectionString!);
                if (!errorMsg.IsNullOrEmpty())
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
    /// 选择 Class 文件保存路径
    /// </summary>
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


    /// <summary>
    /// 尝试连接 PostgreSQL 数据库
    /// </summary>
    /// <param name="connectionString">PostgreSQL 连接字符串</param>
    /// <returns>错误信息，为空则表示连接成功</returns>
    private static async Task<string?> TryConnectPostgreSqlAsync(string connectionString)
    {
        NpgsqlConnection? conn = null;
        try
        {
            if (connectionString.StartsWith("postgres://"))
            {
                connectionString = DbConnectionTool.GetPgSqlConnStr(connectionString);
            }

            await using (conn = new NpgsqlConnection(connectionString))
            {
                await conn.OpenAsync();
            }

            return null;
        }
        catch (Exception ex)
        {
            if (conn != null && conn.State != ConnectionState.Closed)
            {
                await conn.CloseAsync();
            }

            return ex.Message;
        }
    }

    #endregion
}