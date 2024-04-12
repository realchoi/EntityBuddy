namespace DBuddy.Service.Services;

/// <summary>
/// C# 实体类服务
/// </summary>
public interface ICSharpEntityService
{
    /// <summary>
    /// 从 PostgreSQL 数据库生成实体内容
    /// </summary>
    /// <param name="connectionString">PostgreSQL 连接字符串</param>
    /// <param name="schema">架构名</param>
    /// <param name="table">表名</param>
    /// <returns>Class 文件内容，为空则表示未查询到传入的表</returns>
    Task<string?> GenerateFromPostgreSql(string connectionString, string schema, string table);

    /// <summary>
    /// 从 MySQL 数据库生成实体内容
    /// </summary>
    /// <param name="connectionString">PostgreSQL 连接字符串</param>
    /// <param name="schema">架构名</param>
    /// <param name="table">表名</param>
    /// <returns>Class 文件内容，为空则表示未查询到传入的表</returns>
    Task<string?> GenerateFromMySql(string connectionString, string schema, string table);
}