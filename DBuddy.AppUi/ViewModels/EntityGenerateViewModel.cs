using AvaloniaEdit.Utils;
using DBuddy.Infrastructure.Tools;
using DBuddy.Model.Enums;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DBuddy.AppUi.ViewModels
{
    public class EntityGenerateViewModel : ViewModelBase
    {
        public EntityGenerateViewModel()
        {
            _databaseTypes.AddRange(EnumTool.GetAllMembers<DatabaseType>().Where(item => item.Value != 0));
            _programmingLangTypes.AddRange(EnumTool.GetAllMembers<ProgrammingLangType>().Where(item => item.Value != 0));
        }

        private ObservableCollection<EnumMember<DatabaseType>> _databaseTypes = [];
        public ObservableCollection<EnumMember<DatabaseType>> DatabaseTypes => _databaseTypes;

        private ObservableCollection<EnumMember<ProgrammingLangType>> _programmingLangTypes = [];
        public ObservableCollection<EnumMember<ProgrammingLangType>> ProgrammingLangTypes => _programmingLangTypes;

        #region 数据库类型
        private DatabaseType _databaseType = DatabaseType.Unknown;
        public DatabaseType DatabaseType
        {
            get => _databaseType;
            set => this.RaiseAndSetIfChanged(ref _databaseType, value);
        }
        #endregion

        #region 编程语言类型
        private ProgrammingLangType _programmingLangType = ProgrammingLangType.Unknown;
        public ProgrammingLangType ProgrammingLangType
        {
            get => _programmingLangType;
            set => this.RaiseAndSetIfChanged(ref _programmingLangType, value);
        }
        #endregion
    }
}
