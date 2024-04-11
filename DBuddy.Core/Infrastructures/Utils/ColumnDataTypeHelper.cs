namespace DBuddy.Service.Infrastructures.Utils;

/// <summary>
/// 数据库表字段数据类型工具类
/// </summary>
public static class ColumnDataTypeHelper
{
    /// <summary>
    /// 将 PostgreSQL 数据类型转换为 C# 的数据类型
    /// </summary>
    /// <param name="dataType"></param>
    /// <param name="isNullable"></param>
    /// <returns></returns>
    public static string? ConvertPostgreSqlColumnDataTypeToCSharp(string dataType, string isNullable)
    {
        return PostgreSqlToCSharpDataTypeMapping
            .FirstOrDefault(c =>
                c.dataTypes.Contains(dataType) &&
                (string.Equals(c.isNullable, isNullable, StringComparison.OrdinalIgnoreCase) || c.isNullable == null))
            .clrType;
    }


    /// <summary>
    /// 将 MySQL 数据类型转换为 C# 的数据类型
    /// </summary>
    /// <param name="dataType"></param>
    /// <param name="isNullable"></param>
    /// <returns></returns>
    public static string? ConvertMySqlColumnDataTypeToCSharp(string dataType, string isNullable)
    {
        return MySqlToCSharpDataTypeMapping
            .FirstOrDefault(c =>
                c.dataTypes.Contains(dataType) &&
                (string.Equals(c.isNullable, isNullable, StringComparison.OrdinalIgnoreCase) || c.isNullable == null))
            .clrType;
    }


    /// <summary>
    /// PostgreSQL - C# 字段类型映射
    /// </summary>
    private static readonly List<(string clrType, string[] dataTypes, string? isNullable)>
        PostgreSqlToCSharpDataTypeMapping =
        [
            ("string", ["bpchar", "varchar", "text", "point", "json", "jsonb", "xml"], null),
            ("long", ["int8"], "NO"),
            ("long?", ["int8"], "YES"),
            ("int", ["int4", "int2"], "NO"),
            ("int?", ["int4", "int2"], "YES"),
            ("double", ["float8"], "NO"),
            ("double?", ["float8"], "YES"),
            ("float", ["float4"], "NO"),
            ("float?", ["float4"], "YES"),
            ("decimal", ["numeric"], "NO"),
            ("decimal?", ["numeric"], "YES"),
            ("DateTime", ["timestamp", "date"], "NO"),
            ("DateTime?", ["timestamp", "date"], "YES"),
            ("bool", ["bool"], "NO"),
            ("bool?", ["bool"], "YES"),
            ("int[]", ["_int4"], "NO"),
            ("string[]", ["_text"], "NO")
        ];


    /// <summary>
    /// PostgreSQL - C# 字段类型映射
    /// </summary>
    private static readonly List<(string clrType, string[] dataTypes, string? isNullable)>
        MySqlToCSharpDataTypeMapping =
        [
            ("string", ["bpchar", "varchar", "text", "point", "json", "jsonb", "xml"], null),
            ("long", ["int8"], "NO"),
            ("long?", ["int8"], "YES"),
            ("int", ["int4", "int2"], "NO"),
            ("int?", ["int4", "int2"], "YES"),
            ("double", ["float8"], "NO"),
            ("double?", ["float8"], "YES"),
            ("float", ["float4"], "NO"),
            ("float?", ["float4"], "YES"),
            ("decimal", ["numeric"], "NO"),
            ("decimal?", ["numeric"], "YES"),
            ("DateTime", ["timestamp", "date"], "NO"),
            ("DateTime?", ["timestamp", "date"], "YES"),
            ("bool", ["bool"], "NO"),
            ("bool?", ["bool"], "YES"),
            ("int[]", ["_int4"], "NO"),
            ("string[]", ["_text"], "NO")
        ];
}