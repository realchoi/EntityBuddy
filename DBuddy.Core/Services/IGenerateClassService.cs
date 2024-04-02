namespace DBuddy.Service.Services;

public interface IGenerateClassService
{
    /// <summary>
    /// 从 PostgreSQL 数据库生成 Class 文件
    /// </summary>
    /// <param name="connectionString">PostgreSQL 连接字符串</param>
    /// <param name="schema">架构名</param>
    /// <param name="table">表名</param>
    /// <returns>Class 文件内容，为空则表示未查询到传入的表</returns>
    Task<string?> GenerateClassFromPostgreSql(string connectionString, string schema, string table);
}