using AvaloniaEdit.Utils;
using DBuddy.Infrastructure.Tools;
using DBuddy.Model.Enums;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using DBuddy.Model;

namespace DBuddy.AppUi.ViewModels;

public class EntityGenerateViewModel : ViewModelBase
{
    public EntityGenerateViewModel()
    {
        DatabaseTypes.AddRange(EnumTool.GetAllMembers<DatabaseType>().Where(item => item.Value != 0)
            .Select(item => new ComboBoxItemDto<int>((int)item.Value, item.Description)));
        ProgrammingLanguages.AddRange(EnumTool.GetAllMembers<ProgrammingLanguage>().Where(item => item.Value != 0)
            .Select(item => new ComboBoxItemDto<int>((int)item.Value, item.Description)));
    }

    /// <summary>
    /// 数据库类型下拉选项
    /// </summary>
    public ObservableCollection<ComboBoxItemDto<int>> DatabaseTypes { get; } = [];

    /// <summary>
    /// 编程语言下拉选项
    /// </summary>
    public ObservableCollection<ComboBoxItemDto<int>> ProgrammingLanguages { get; } = [];

    #region 数据库类型

    private DatabaseType _databaseType = DatabaseType.Unknown;

    public DatabaseType DatabaseType
    {
        get => _databaseType;
        set => this.RaiseAndSetIfChanged(ref _databaseType, value);
    }

    #endregion

    #region 编程语言类型

    private ProgrammingLanguage _programmingLanguage = ProgrammingLanguage.Unknown;

    public ProgrammingLanguage ProgrammingLanguage
    {
        get => _programmingLanguage;
        set => this.RaiseAndSetIfChanged(ref _programmingLanguage, value);
    }

    #endregion
}