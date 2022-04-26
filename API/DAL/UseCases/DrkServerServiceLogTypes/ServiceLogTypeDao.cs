using System;
using System.Collections.Generic;
using System.Linq;
using API.BLL.Base;
using API.BLL.Helper;
using API.BLL.UseCases.DrkServerServiceLogTypes.Daos;
using API.BLL.UseCases.DrkServerServiceLogTypes.Entities;
using API.BLL.UseCases.DrkServerServiceLogTypes.Transformer;
using Dapper;
using DapperExtensions;
using DapperExtensions.Sql;
using Microsoft.Extensions.Options;
using Npgsql;

namespace API.DAL.UseCases.DrkServerServiceLogTypes
{
    public class ServiceLogTypeDao : IServiceLogTypeDao
    {
        private readonly string ConnectionString;
        private readonly string TableName = "servicelogtype";
        private ServiceLogTypeTransformer Transformer;

        public ServiceLogTypeDao(IOptions<AppSettings> appSettings)
        {
            ConnectionString = appSettings.Value.DbConnection;
            Transformer = new ServiceLogTypeTransformer();
        }
        
        public bool DeleteAll()
        {
            DapperExtensions.DapperExtensions.SqlDialect = new PostgreSqlDialect();
            try
            {
                using var con = new NpgsqlConnection(ConnectionString);
                con.Open();

                var affectedRows = con.Execute(
                    $@"
                        DELETE FROM {TableName}
                    ",
                    new
                    {
                    }
                );
                return affectedRows > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void CreateMany(List<ServiceLogType> entities)
        {
            var entries = entities.Select(x => Transformer.ToDbEntity(x)).ToList();
            DapperExtensions.DapperExtensions.SqlDialect = new PostgreSqlDialect();
            using var con = new NpgsqlConnection(ConnectionString);
            con.Open();
            con.Insert<DbServiceLogType>(entries);
        }
        
        
        public DataTableSearchResult<ServiceLogType> FindBySearchValue(ServiceLogTypeSearchOptions searchOptions)
        {
            var query = $@"WITH TempResult AS (SELECT * FROM {TableName} ";
            var joins = new List<string>();

            var queryFilter = new HashSet<string>();
            var queryOrder = "";

            var queryParams = new
            {
                id = $@"%{searchOptions.Id}%",
                listId = $@"%{searchOptions.ListId}%",
                shortcut = $@"%{searchOptions.Shortcut}%",
                name = $@"%{searchOptions.Name}%",
                skip = searchOptions.Skip,
                take = searchOptions.Take
            };

            if (!string.IsNullOrEmpty(searchOptions.Id))
            {
                queryFilter.Add($@"{TableName}.Id ILIKE @id ");
            }

            if (!string.IsNullOrEmpty(searchOptions.ListId))
            {
                queryFilter.Add($@"{TableName}.ListId ILIKE @listId ");
            }

            if (!string.IsNullOrEmpty(searchOptions.Name))
            {
                queryFilter.Add($@"{TableName}.Name ILIKE @name ");
            }

            if (!string.IsNullOrEmpty(searchOptions.Shortcut))
            {
                queryFilter.Add($@"{TableName}.Shortcut ILIKE @shortcut ");
            }

            switch (searchOptions.SortColumn)
            {
                case ServiceLogTypeSortColumn.Id:
                    queryOrder = $@"ORDER BY {TableName}.Id {GetSearchDirection(searchOptions.IsDescending)} ";
                    break;
                case ServiceLogTypeSortColumn.ListId:
                    queryOrder = $@"ORDER BY {TableName}.ListId {GetSearchDirection(searchOptions.IsDescending)} ";
                    break;
                case ServiceLogTypeSortColumn.Shortcut:
                    queryOrder = $@"ORDER BY {TableName}.Shortcut {GetSearchDirection(searchOptions.IsDescending)} ";
                    break;
                case ServiceLogTypeSortColumn.Name:
                    queryOrder = $@"ORDER BY {TableName}.Name {GetSearchDirection(searchOptions.IsDescending)} ";
                    break;
                default:
                    queryOrder = $@"ORDER BY {TableName}.Id {GetSearchDirection(searchOptions.IsDescending)} ";
                    break;
            }

            if (joins.Count > 0)
                query += String.Join(' ', joins.ToHashSet());

            if (queryFilter.Count > 0)
                query += "WHERE " + string.Join("AND ", queryFilter);

            query += queryOrder;
            query += $@"), TempCount AS (
                        SELECT COUNT(*) AS TotalRowCount FROM TempResult
                        ) 
                    SELECT * FROM TempResult as data, TempCount AS meta
                    Offset @skip Rows
                    Fetch Next @take Rows Only ";

            DapperExtensions.DapperExtensions.SqlDialect = new PostgreSqlDialect();

            var totalCount = (Int64)0;
            using var con = new NpgsqlConnection(ConnectionString);
            con.Open();
            var res = con.Query<ServiceLogType>(
                query,
                new[]
                {
                    typeof(DbServiceLogType), typeof(Int64)
                },
                (objects) =>
                {
                    var res = (DbServiceLogType)objects[0];
                    totalCount = (Int64)objects[1];
                    return Transformer.ToEntity(res);
                },
                queryParams,
                splitOn: "ident,TotalRowCount"
            ).ToList();
            return new DataTableSearchResult<ServiceLogType>()
            {
                Data = res,
                TotalRowCount = totalCount
            };
        }

        private string GetSearchDirection(bool? isDescending) => isDescending == true ? "DESC" : "ASC";
    }
}