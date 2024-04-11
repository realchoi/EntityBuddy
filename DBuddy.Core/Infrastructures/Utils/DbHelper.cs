using System.Data;
using MySql.Data.MySqlClient;
using Npgsql;
using SpringMountain.Infrastructure.Tools;

namespace DBuddy.Service.Infrastructures.Utils;

/// <summary>
/// 数据库相关工具类
/// </summary>
public static class DbHelper
{
    /// <summary>
    /// 尝试连接 PostgreSQL 数据库
    /// </summary>
    /// <param name="connectionString">PostgreSQL 连接字符串</param>
    /// <returns>错误信息，为空则表示连接成功</returns>
    public static async Task<string?> TryConnectPostgreSqlAsync(string connectionString)
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
            return ex.Message;
        }
        finally
        {
            if (conn != null && conn.State != ConnectionState.Closed)
            {
                await conn.CloseAsync();
            }
        }
    }


    /// <summary>
    /// 尝试连接 MySQL 数据库
    /// </summary>
    /// <param name="connectionString">PostgreSQL 连接字符串</param>
    /// <returns>错误信息，为空则表示连接成功</returns>
    public static async Task<string?> TryConnectMySqlAsync(string connectionString)
    {
        MySqlConnection? conn = null;
        try
        {
            await using (conn = new MySqlConnection(connectionString))
            {
                await conn.OpenAsync();
            }

            return null;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            if (conn != null && conn.State != ConnectionState.Closed)
            {
                await conn.CloseAsync();
            }
        }
    }
}