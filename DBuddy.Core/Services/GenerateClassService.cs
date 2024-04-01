using Dapper;
using Npgsql;
using SpringMountain.Infrastructure.Tools;

namespace DBuddy.Service.Services;

public class GenerateClassService : IGenerateClassService
{
    /// <summary>
    /// 从 PostgreSQL 数据库生成 Class 文件
    /// </summary>
    /// <param name="connectionString">PostgreSQL 连接字符串</param>
    /// <param name="schema">架构名</param>
    /// <param name="table">表名</param>
    /// <returns>Class 文件内容</returns>
    public async Task<string> GenerateClassFromPostgreSql(string connectionString, string schema, string table)
    {
        if (connectionString.StartsWith("postgres://"))
        {
            connectionString = DbConnectionTool.GetPgSqlConnStr(connectionString);
        }

        await using var conn = new NpgsqlConnection(connectionString);
        // var sql = "";
        // var x = await conn.QueryAsync<object>(sql);
        return "还没写好呢";
    }
}