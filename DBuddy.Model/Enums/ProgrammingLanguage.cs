using System.ComponentModel;

namespace DBuddy.Model.Enums;

/// <summary>
/// 编程语言枚举
/// </summary>
public enum ProgrammingLanguage
{
    /// <summary>
    /// 未知类型
    /// </summary>
    [Description("未知类型")]
    Unknown = 0,

    /// <summary>
    /// C#
    /// </summary>
    [Description("C#")]
    CSharp = 1,

    /// <summary>
    /// Java
    /// </summary>
    [Description("Java")]
    Java = 2,

    /// <summary>
    /// TypeScript
    /// </summary>
    [Description("TypeScript")]
    TypeScript = 3
}