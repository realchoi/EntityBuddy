﻿using System.Text;
using Dapper;
using DBuddy.Model.Dtos;
using DBuddy.Service.Infrastructures.Utils;
using Npgsql;
using SpringMountain.Api.Exceptions.Contracts;

namespace DBuddy.Service.Services;

public class GenerateClassService : IGenerateClassService
{
    /// <summary>
    /// 从 PostgreSQL 数据库生成 Class 文件
    /// </summary>
    /// <param name="connectionString">PostgreSQL 连接字符串</param>
    /// <param name="schema">架构名</param>
    /// <param name="table">表名</param>
    /// <returns>Class 文件内容，为空则表示未查询到传入的表</returns>
    public async Task<string?> GenerateClassFromPostgreSql(string connectionString, string schema, string table)
    {
        var errorMessage = await DbHelper.TryConnectPostgreSqlAsync(connectionString);
        if (errorMessage != null)
            throw new InvalidParameterException($"数据库连接失败：{errorMessage}");

        await using var conn = new NpgsqlConnection(connectionString);
        const string sql = """
                           SELECT c.column_name AS ColumnName,
                                  c.udt_name AS UdtName,
                                  c.is_nullable AS IsNullable,
                                  pgd.description AS Comment
                           FROM information_schema.columns c
                                    LEFT JOIN pg_catalog.pg_description pgd
                                              ON pgd.objoid = (SELECT attrelid
                                                               FROM pg_catalog.pg_class pgc
                                                                        JOIN pg_catalog.pg_namespace pgn ON pgn.oid = pgc.relnamespace
                                                                        JOIN pg_catalog.pg_attribute pgat ON pgat.attrelid = pgc.oid
                                                               WHERE pgn.nspname = @schema
                                                                 AND pgc.relname = @table
                                                                 AND pgat.attnum = c.ordinal_position)
                                                  AND pgd.objsubid = c.ordinal_position
                           WHERE c.table_schema = @schema
                             AND c.table_name = @table;
                           """;
        const string template = """
                                using System;
                                using System.Collections.Generic;
                                using System.Linq;
                                using System.Text;

                                namespace DBuddy.Example
                                {
                                @CONTENT
                                }
                                """;
        var content = new StringBuilder();
        var columns = (await conn.QueryAsync<TableColumnDto>(sql, new { schema, table })).ToList();
        if (columns.Count == 0)
        {
            return null;
        }

        var index = 0;
        foreach (var column in columns)
        {
            var comment = $"""
                               /// <summary>
                               /// {column.Comment}
                               /// </summary>
                           """;
            content.AppendLine(comment);
            var dataType = ColumnDataTypeHelper.ConvertPostgreSqlColumnDataType(column.UdtName, column.IsNullable);
            if (dataType == null)
                throw new ApiBaseException($"无法识别的数据类型：{column.UdtName}");

            content.Append("    public " + dataType + " " + column.ColumnName.ToPascalCase() + " { get; set; }");
            if (index < columns.Count - 1)
            {
                content.AppendLine();
                content.AppendLine();
            }

            index++;
        }

        var result = template.Replace("@CONTENT", content.ToString());
        return result;
    }
}