using System.Text.RegularExpressions;

namespace DBuddy.Service.Infrastructures.Utils;

/// <summary>
/// 字符串处理工具类
/// </summary>
public static class StringHelper
{
    /// <summary>
    /// 将字符串从下划线命名风格或者小驼峰命名风格转换为大驼峰命名风格
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToPascalCase(string str)
    {
        // 将下划线转换为空格
        str = Regex.Replace(str, @"_", " ");

        // 将每个单词的第一个字母大写
        str = Regex.Replace(str, @"\b\w", m => m.Value.ToUpper());

        // 删除所有空格
        str = Regex.Replace(str, @"\s", "");

        // 将第一个字母大写
        str = str.Substring(0, 1).ToUpper() + str.Substring(1);

        return str;
    }
}