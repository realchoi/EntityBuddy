using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace DBuddy.Service.Infrastructures.Utils;

/// <summary>
/// 弹窗工具类
/// </summary>
public static class MessageBoxUtil
{
    /// <summary>
    /// 显示弹窗消息
    /// </summary>
    /// <param name="title">弹窗标题</param>
    /// <param name="message">弹窗内容</param>
    /// <param name="icon">弹窗图标</param>
    /// <param name="button">弹窗按钮</param>
    public static async Task ShowMessageBox(string title, string message,
        Icon icon = Icon.Info, ButtonEnum button = ButtonEnum.Ok)
    {
        var msBox = MessageBoxManager.GetMessageBoxStandard(title, message, button, icon);
        await msBox.ShowAsync();
    }
}