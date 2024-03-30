using System;
using System.Data;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using DBuddy.Infrastructure.Tools;
using DBuddy.Model;
using DBuddy.Model.Enums;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Npgsql;

namespace DBuddy.AppUi.Views;

public partial class EntityGenerateView : UserControl
{
    public EntityGenerateView()
    {
        InitializeComponent();
    }

    #region 事件方法

    /// <summary>
    /// 测试数据库连接
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Btn_TestConnect_OnClick(object? sender, RoutedEventArgs e)
    {
        var connectionString = ConnectionStringTextBox.Text;
        if (connectionString.IsNullOrWhiteSpace())
        {
            var infoBox = MessageBoxManager.GetMessageBoxStandard("提示", "连接字符串不能为空！", ButtonEnum.Ok, Icon.Warning);
            await infoBox.ShowAsync();
            return;
        }

        var databaseType = DatabaseTypeComboBox.SelectedValue as ComboBoxItemDto<int>;
        var databaseTypeEnum = (DatabaseType)databaseType!.Value;
        switch (databaseTypeEnum)
        {
            case DatabaseType.PostgreSql:
                var errorMsg = await TryConnectPostgreSqlAsync(connectionString!);
                if (!errorMsg.IsNullOrEmpty())
                {
                    var errorBox = MessageBoxManager.GetMessageBoxStandard("提示", $"连接失败！\r\n{errorMsg}",
                        ButtonEnum.Ok, Icon.Error);
                    await errorBox.ShowAsync();
                }
                else
                {
                    var successBox = MessageBoxManager.GetMessageBoxStandard("提示", "连接成功！\r\n现在可以生成实体了。",
                        ButtonEnum.Ok, Icon.Success);
                    await successBox.ShowAsync();
                }

                break;
            case DatabaseType.Unknown:
            case DatabaseType.Oracle:
            case DatabaseType.MySql:
            case DatabaseType.SqlServer:
            default:
                break;
        }
    }

    #endregion


    #region 私有方法

    /// <summary>
    /// 尝试连接 PostgreSQL 数据库
    /// </summary>
    /// <param name="connectionString">PostgreSQL 连接字符串</param>
    /// <returns>错误信息，为空则表示连接成功</returns>
    private static async Task<string?> TryConnectPostgreSqlAsync(string connectionString)
    {
        NpgsqlConnection? conn = null;
        try
        {
            if (connectionString.StartsWith("postgres://"))
            {
                connectionString = DbConnTool.GetPgSqlConnStr(connectionString);
            }

            await using (conn = new NpgsqlConnection(connectionString))
            {
                await conn.OpenAsync();
            }

            return null;
        }
        catch (Exception ex)
        {
            if (conn != null && conn.State != ConnectionState.Closed)
            {
                await conn.CloseAsync();
            }

            return ex.Message;
        }
    }

    #endregion
}