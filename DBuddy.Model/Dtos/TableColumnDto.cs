using Newtonsoft.Json;

namespace DBuddy.Model.Dtos;

public class TableColumnDto
{
    /// <summary>
    /// 字段名
    /// </summary>
    [JsonProperty("column_name")]
    public string ColumnName { get; set; }

    /// <summary>
    /// 数据类型
    /// </summary>
    [JsonProperty("data_type")]
    public string DataType { get; set; }

    /// <summary>
    /// 是否可空
    /// </summary>
    [JsonProperty("nullable")]
    public string Nullable { get; set; }
}