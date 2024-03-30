using System.ComponentModel;

namespace DBuddy.Model.Enums;

/// <summary>
/// 数据库类型枚举
/// </summary>
public enum DatabaseType
{
    /// <summary>
    /// 未知类型
    /// </summary>
    [Description("未知类型")]
    Unknown = 0,

    /// <summary>
    /// PostgreSQL
    /// </summary>
    [Description("PostgreSQL")]
    PostgreSql = 1,

    /// <summary>
    /// Oracle
    /// </summary>
    [Description("Oracle")]
    Oracle = 2,

    /// <summary>
    /// MySQL
    /// </summary>
    [Description("MySQL")]
    MySql = 3,

    /// <summary>
    /// SQLServer
    /// </summary>
    [Description("SQL Server")]
    SqlServer = 4
}