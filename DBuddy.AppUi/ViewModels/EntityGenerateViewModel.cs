using AvaloniaEdit.Utils;
using DBuddy.Infrastructure.Tools;
using DBuddy.Model.Enums;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;

namespace DBuddy.AppUi.ViewModels;

public class EntityGenerateViewModel : ViewModelBase
{
    public EntityGenerateViewModel()
    {
        _databaseTypes.AddRange(EnumTool.GetAllMembers<DatabaseType>().Where(item => item.Value != 0));
        _programmingLanguages.AddRange(EnumTool.GetAllMembers<ProgrammingLanguage>().Where(item => item.Value != 0));
    }

    private ObservableCollection<EnumMember<DatabaseType>> _databaseTypes = [];
    public ObservableCollection<EnumMember<DatabaseType>> DatabaseTypes => _databaseTypes;

    private ObservableCollection<EnumMember<ProgrammingLanguage>> _programmingLanguages = [];
    public ObservableCollection<EnumMember<ProgrammingLanguage>> ProgrammingLanguages => _programmingLanguages;

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