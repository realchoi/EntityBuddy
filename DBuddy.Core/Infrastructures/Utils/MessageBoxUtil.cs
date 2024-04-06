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
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="button"></param>
    /// <param name="icon"></param>
    public static async Task ShowMessageBox(string title, string message,
        ButtonEnum button = ButtonEnum.Ok, Icon icon = Icon.Info)
    {
        var msBox = MessageBoxManager.GetMessageBoxStandard(title, message, button, icon);
        await msBox.ShowAsync();
    }
}