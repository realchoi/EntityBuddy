using System.Threading.Tasks;
using DBuddy.Model.Enums;

namespace DBuddy.AppUi.Services;

public interface IEntityGenerateService
{
    /// <summary>
    /// 生成实体类的内容
    /// </summary>
    /// <param name="lang">编程语言</param>
    /// <param name="dbType">数据库类型</param>
    /// <param name="connectionString">数据库连接字符串</param>
    /// <param name="schema">架构名</param>
    /// <param name="table">表名</param>
    /// <returns>fileName：文件名，fileContent：实体类的内容</returns>
    Task<(string? fileName, string? fileContent)> GenerateEntityClassContent(
        ProgrammingLanguage lang, DatabaseType dbType, string connectionString, string schema, string table);
}