using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;

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
    public static async Task ShowMessageBox(string title, string message, Icon icon = Icon.Info)
    {
        var box = MessageBoxManager.GetMessageBoxCustom(
            new MessageBoxCustomParams
            {
                ButtonDefinitions = new List<ButtonDefinition>
                {
                    new() { Name = "好的" }
                },
                ContentTitle = title,
                ContentMessage = message,
                Icon = icon,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                CanResize = false,
                MaxWidth = 500,
                MaxHeight = 800,
                SizeToContent = SizeToContent.WidthAndHeight,
                ShowInCenter = true,
                Topmost = false
            });
        await box.ShowAsync();
    }
}