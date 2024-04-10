using System;
using System.Threading.Tasks;
using DBuddy.Model.Enums;
using DBuddy.Service.Infrastructures.Utils;
using DBuddy.Service.Services;
using SpringMountain.Api.Exceptions.Contracts;

namespace DBuddy.AppUi.Services;

public class EntityGenerateService : IEntityGenerateService
{
    /// <summary>
    /// 生成实体类的内容
    /// </summary>
    /// <param name="lang">编程语言</param>
    /// <param name="dbType">数据库类型</param>
    /// <param name="connectionString">数据库连接字符串</param>
    /// <param name="schema">架构名</param>
    /// <param name="table">表名</param>
    /// <returns>fileName：文件名，fileContent：实体类的内容</returns>
    /// <exception cref="ApiBaseException"></exception>
    public async Task<(string? fileName, string? fileContent)> GenerateEntityClassContent(
        ProgrammingLanguage lang, DatabaseType dbType, string connectionString, string schema, string table)
    {
        // 文件名后缀
        var fileName = StringHelper.ToPascalCase(table);
        // 最终生成的实体类的内容
        string? fileContent;
        try
        {
            switch (lang)
            {
                case ProgrammingLanguage.CSharp:
                    fileName += ".cs";
                    var cSharpEntityService = App.GetService<ICSharpEntityService>();
                    switch (dbType)
                    {
                        case DatabaseType.PostgreSql:
                            fileContent = await cSharpEntityService.GenerateEntityClassContentFromPostgreSql
                                (connectionString, schema, table);
                            break;
                        case DatabaseType.Oracle:
                            fileContent = null;
                            throw new InvalidParameterException("Oracle 暂未实现");
                            break;
                        case DatabaseType.MySql:
                            fileContent = null;
                            throw new InvalidParameterException("MySql 暂未实现");
                            break;
                        case DatabaseType.SqlServer:
                            fileContent = null;
                            throw new InvalidParameterException("SqlServer 暂未实现");
                            break;
                        case DatabaseType.Unknown:
                        default:
                            throw new InvalidParameterException("未知的数据库类型");
                    }

                    break;
                case ProgrammingLanguage.Java:
                    fileName += ".java";
                    fileContent = null;
                    throw new InvalidParameterException("Java 暂未实现");
                    break;
                case ProgrammingLanguage.TypeScript:
                    fileName += ".ts";
                    fileContent = null;
                    throw new InvalidParameterException("TypeScript 暂未实现");
                    break;
                case ProgrammingLanguage.Unknown:
                default:
                    throw new InvalidParameterException("未知的编程语言");
            }
        }
        catch (Exception e)
        {
            throw new ApiBaseException($"生成失败！\r\n{e.Message}");
        }

        if (fileName == null)
        {
            throw new ApiBaseException("生成的文件名为空，请联系开发人员！");
        }

        if (fileContent == null)
        {
            throw new ApiBaseException("生成的内容为空，请检查架构名和表名是否正确！");
        }

        return (fileName, fileContent);
    }
}