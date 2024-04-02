namespace DBuddy.Model.Enums;

/// <summary>
/// PostgreSQL 数据库中，表字段数据类型
/// </summary>
public enum PostgreSqlColumnDataType
{
    /// <summary>
    /// 字符串（"bpchar", "varchar", "text", "point"）
    /// </summary>
    String = 1,

    /// <summary>
    /// int 或 long（"int8", "int4", "int2"）
    /// </summary>
    Long,

    /// <summary>
    /// 小数，如 float 等（"float8", "numeric", "float4"）
    /// </summary>
    Decimal,

    /// <summary>
    /// 时间（"timestamp", "date"）
    /// </summary>
    DateTime,

    /// <summary>
    /// json，jsonb（"json", "jsonb"）
    /// </summary>
    Json,

    /// <summary>
    /// bool（"bool"）
    /// </summary>
    Boolean,

    /// <summary>
    /// xml（"xml"）
    /// </summary>
    Xml,

    /// <summary>
    /// 数组（"_int4", "_text"）
    /// </summary>
    Array
}