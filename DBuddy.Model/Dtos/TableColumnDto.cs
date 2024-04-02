namespace DBuddy.Model.Dtos;

public class TableColumnDto
{
    /// <summary>
    /// 字段名
    /// </summary>
    public string ColumnName { get; set; }

    /// <summary>
    /// 数据类型
    /// </summary>
    public string UdtName { get; set; }

    /// <summary>
    /// 是否可空
    /// </summary>
    public string IsNullable { get; set; }

    /// <summary>
    /// 字段注释
    /// </summary>
    public string Comment { get; set; }
}