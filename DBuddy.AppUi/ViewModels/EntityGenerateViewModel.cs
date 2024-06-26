﻿using System;
using System.Collections.Generic;
using DBuddy.Model.Enums;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Web;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using DBuddy.AppUi.Services;
using DBuddy.Model;
using DBuddy.Service.Infrastructures.Utils;
using MsBox.Avalonia.Enums;
using SpringMountain.Infrastructure.Tools;

namespace DBuddy.AppUi.ViewModels;

public class EntityGenerateViewModel : ViewModelBase
{
    public EntityGenerateViewModel()
    {
        InitComboBoxSource();

        _selectedProgrammingLanguage = ProgrammingLanguages.First();
        _selectedDatabaseType = DatabaseTypes.First();
        _connectionString = string.Empty;
        _schemaName = string.Empty;
        _tableName = string.Empty;
        _saveClassFilePath = string.Empty;

        // 初始化事件命令
        TestDbConnectCommand = ReactiveCommand.Create(TestDbConnect);
        SelectSaveClassFilePathCommand = ReactiveCommand.Create(SelectSaveClassFilePath);
        GenerateClassFileCommand = ReactiveCommand.Create(GenerateClassFile);
        CloseDialogCommand = ReactiveCommand.Create(CloseDialog);
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
    /// 架构名称
    /// </summary>
    private string _schemaName;

    /// <summary>
    /// 架构名称
    /// </summary>
    public string SchemaName
    {
        get => _schemaName;
        set => this.RaiseAndSetIfChanged(ref _schemaName, value);
    }

    /// <summary>
    /// 表名称
    /// </summary>
    private string _tableName;

    /// <summary>
    /// 表名称
    /// </summary>
    public string TableName
    {
        get => _tableName;
        set => this.RaiseAndSetIfChanged(ref _tableName, value);
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

    /// <summary>
    /// 关闭弹窗 Command
    /// </summary>
    public ReactiveCommand<Unit, Unit> CloseDialogCommand { get; }

    #endregion

    #region 私有方法

    /// <summary>
    /// 测试数据库连接
    /// </summary>
    private async void TestDbConnect()
    {
        if (SelectedDatabaseType.Value < 0)
        {
            await MessageBoxUtil.ShowMessageBox("提示", "请选择数据库类型！", Icon.Warning);
            return;
        }

        if (ConnectionString.IsNullOrWhiteSpace())
        {
            await MessageBoxUtil.ShowMessageBox("提示", "连接字符串不能为空！", Icon.Warning);
            return;
        }

        var databaseTypeEnum = (DatabaseType)SelectedDatabaseType.Value;
        switch (databaseTypeEnum)
        {
            case DatabaseType.PostgreSql:
                var errorMsg = await DbHelper.TryConnectPostgreSqlAsync(ConnectionString);
                if (errorMsg != null)
                {
                    await MessageBoxUtil.ShowMessageBox("提示", $"连接失败！\r\n{errorMsg}", Icon.Error);
                }
                else
                {
                    await MessageBoxUtil.ShowMessageBox("提示", "连接成功！\r\n现在可以生成实体了。", Icon.Success);
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
        if (SelectedProgrammingLanguage.Value < 0)
        {
            await MessageBoxUtil.ShowMessageBox("提示", "请选择编程语言！", Icon.Warning);
            return;
        }

        if (SelectedDatabaseType.Value < 0)
        {
            await MessageBoxUtil.ShowMessageBox("提示", "请选择数据库类型！", Icon.Warning);
            return;
        }

        if (ConnectionString.IsNullOrWhiteSpace())
        {
            await MessageBoxUtil.ShowMessageBox("提示", "连接字符串不能为空！", Icon.Warning);
            return;
        }

        if (SchemaName.IsNullOrWhiteSpace())
        {
            // 为空则默认使用 public
            SchemaName = "public";
        }

        if (TableName.IsNullOrWhiteSpace())
        {
            await MessageBoxUtil.ShowMessageBox("提示", "表名不能为空！", Icon.Warning);
            return;
        }

        if (SaveClassFilePath.IsNullOrWhiteSpace())
        {
            // 默认保存到桌面
            SaveClassFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        var lang = (ProgrammingLanguage)SelectedProgrammingLanguage.Value;
        var dbType = (DatabaseType)SelectedDatabaseType.Value;
        var entityGenerateService = App.GetService<IEntityGenerateService>();
        try
        {
            var (fileName, fileContent) = await entityGenerateService.GenerateEntityClassContent(
                lang, dbType, ConnectionString, SchemaName, TableName
            );
            var filePath = Path.Combine(SaveClassFilePath, fileName!);
            await File.WriteAllTextAsync(filePath, fileContent!);
            await MessageBoxUtil.ShowMessageBox("提示", $"实体生成成功，请查看 {SaveClassFilePath} 下的 {fileName} 文件！",
                Icon.Success);
        }
        catch (Exception e)
        {
            await MessageBoxUtil.ShowMessageBox("提示", e.Message, Icon.Error);
        }
    }


    /// <summary>
    /// 选择 Class 文件保存路径
    /// </summary>
    private async void SelectSaveClassFilePath()
    {
        var suggestedPath = _saveClassFilePath.IsNullOrWhiteSpace()
            ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
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
    /// 关闭当前弹窗
    /// </summary>
    private static void CloseDialog()
    {
        VmLocator.MainWindowViewModel.EntityGenerateViewIsOpened = false;
    }

    /// <summary>
    /// 初始化下拉选项数据源
    /// </summary>
    private void InitComboBoxSource()
    {
        var programmingLanguages = EnumTool.GetAllMembers<ProgrammingLanguage>()
            .Where(item => item.Value != 0)
            .Select(item => new ComboBoxItemDto<int>((int)item.Value, item.Description))
            .ToList();
        if (programmingLanguages.Count != 0)
        {
            ProgrammingLanguages.AddIfNotContains(programmingLanguages);
        }
        else
        {
            ProgrammingLanguages.Add(new ComboBoxItemDto<int>(-1, "请选择编程语言", false));
        }

        var databaseTypes = EnumTool.GetAllMembers<DatabaseType>()
            .Where(item => item.Value != 0)
            .Select(item => new ComboBoxItemDto<int>((int)item.Value, item.Description))
            .ToList();
        if (databaseTypes.Count != 0)
        {
            DatabaseTypes.AddIfNotContains(databaseTypes);
        }
        else
        {
            DatabaseTypes.Add(new ComboBoxItemDto<int>(-1, "请选择数据库类型", false));
        }
    }

    #endregion
}