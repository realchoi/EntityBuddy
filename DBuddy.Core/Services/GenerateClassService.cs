using Dapper;
using DBuddy.Model.Dtos;
using DBuddy.Service.Infrastructures.Utils;
using Newtonsoft.Json;
using Npgsql;
using SpringMountain.Api.Exceptions.Contracts;

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
        var errorMessage = await DbHelper.TryConnectPostgreSqlAsync(connectionString);
        if (errorMessage != null)
            throw new InvalidParameterException($"数据库连接失败：{errorMessage}");

        await using var conn = new NpgsqlConnection(connectionString);
        var sql = $@"SELECT column_name ColumnName, data_type DataType, is_nullable Nullable
FROM information_schema.columns
WHERE table_schema = '{schema}'
  AND table_name = '{table}'";
        var columns = await conn.QueryAsync<TableColumnDto>(sql);
        return JsonConvert.SerializeObject(columns);
    }
}