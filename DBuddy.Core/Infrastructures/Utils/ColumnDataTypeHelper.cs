using DBuddy.Model.Enums;

namespace DBuddy.Service.Infrastructures.Utils;

/// <summary>
/// 数据库表字段数据类型工具类
/// </summary>
public static class ColumnDataTypeHelper
{
    /// <summary>
    /// 从数据库字段的 udtName 转换为 <see cref="PostgreSqlColumnDataType">PropertyTypeEnum</see> 枚举中的某一个数据类型
    /// </summary>
    /// <param name="udtName"></param>
    /// <param name="isNullable"></param>
    /// <returns></returns>
    public static string? ConvertPostgreSqlColumnDataType(string udtName, string isNullable)
    {
        return PostgreSqlClrTypeMapping
            .FirstOrDefault(c =>
                c.udtNames.Contains(udtName) &&
                (string.Equals(c.isNullable, isNullable, StringComparison.OrdinalIgnoreCase) || c.isNullable == null))
            .clrType;
    }

    /// <summary>
    /// 字段类型映射
    /// </summary>
    private static readonly List<(string clrType, string[] udtNames, string? isNullable)> PostgreSqlClrTypeMapping =
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