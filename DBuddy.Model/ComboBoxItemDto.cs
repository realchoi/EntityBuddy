namespace DBuddy.Model;

/// <summary>
/// 下拉选项
/// </summary>
public class ComboBoxItemDto<T>(T value, string text)
{
    /// <summary>
    /// 值
    /// </summary>
    public T Value { get; private set; } = value;

    /// <summary>
    /// 文本
    /// </summary>
    public string Text { get; private set; } = text;
}