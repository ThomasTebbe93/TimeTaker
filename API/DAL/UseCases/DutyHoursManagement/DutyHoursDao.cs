using System;
using System.Collections.Generic;
using System.Linq;
using API.BLL.Base;
using API.BLL.Helper;
using API.BLL.UseCases.DutyHoursManagement.Daos;
using API.BLL.UseCases.DutyHoursManagement.Entities;
using API.BLL.UseCases.DutyHoursManagement.Transformer;
using Dapper;
using DapperExtensions.Sql;
using Microsoft.Extensions.Options;
using Npgsql;

namespace API.DAL.UseCases.DutyHoursManagement
{
    public class DutyHoursDao : AbstractPostgresqlDao<DutyHours, DbDutyHours, DutyHoursIdent, DutyHoursTransformer>,
        IDutyHoursDao
    {
    public DutyHoursDao(IOptions<AppSettings> appSettings) : base( appSettings, "dutyhours")
    {
    }

        public List<DutyHours> GetPersonalBookings(Context context)
        {
            using var con = new NpgsqlConnection(ConnectionString);
            con.Open();

            return con.Query<DbDutyHours>(
                $@"
                        SELECT hours.*
                        FROM {TableName} AS hours
                        LEFT JOIN dutyhoursbookings AS booking 
                            ON booking.Ident = hours.signInBookingIdent 
                        WHERE booking.userident = @userident
                        ORDER BY booking.bookingtime DESC",
                new
                {
                    userident = context.User.Ident.Ident,
                }
            ).Select(x => Transformer.ToEntity(x)).ToList();
        }

        public DataTableSearchResult<DutyHours> FindBySearchValue(Context context, DutyHoursSearchOptions searchOptions)
        {
            var query = $@"WITH TempResult AS (SELECT {TableName}.* FROM {TableName} ";
            var bookingsTable = "dutyhoursbookings";
            var startBookingsJoin =
                $@"LEFT JOIN {bookingsTable} ON {bookingsTable}.Ident = {TableName}.signInBookingIdent ";
            var userTable = "users";
            var userJoin =
                $@"LEFT JOIN {userTable} ON {userTable}.Ident = {bookingsTable}.UserIdent ";
            var queryFilter = new HashSet<string>();
            var queryOrder = "";
            var queryJoins = new HashSet<string>();

            var queryParams = new
            {
                userName = $@"%{searchOptions.UserName}%",
                userIdent = context.User.Ident.Ident,
                skip = searchOptions.Skip,
                take = searchOptions.Take
            };

            var rights = context.User.Role.Rights.Select(x => x.Key).ToHashSet();
            if (!rights.Contains(Rights.DutyHoursDisplaySelf) && !rights.Contains(Rights.DutyHoursDisplayAll))
                return new DataTableSearchResult<DutyHours>();

            if (rights.Contains(Rights.DutyHoursDisplaySelf) && !rights.Contains(Rights.DutyHoursDisplayAll))
            {
                queryFilter.Add($@"{userTable}.Ident = @userIdent ");
                queryJoins.Add(startBookingsJoin);
                queryJoins.Add(userJoin);
            }

            if (!string.IsNullOrEmpty(searchOptions.UserName))
            {
                queryFilter.Add($@"{userTable}.FirstName || ', ' || {userTable}.LastName LIKE @userName ");
                queryJoins.Add(startBookingsJoin);
                queryJoins.Add(userJoin);
            }

            switch (searchOptions.SortColumn)
            {
                case DutyHoursSortColumn.User:
                    queryOrder =
                        $@"ORDER BY {userTable}.FirstName || ', ' || {userTable}.LastName {GetSearchDirection(searchOptions.IsDescending)} ";
                    queryJoins.Add(startBookingsJoin);
                    queryJoins.Add(userJoin);
                    break;

                case DutyHoursSortColumn.Date:
                    queryOrder =
                        $@"ORDER BY {bookingsTable}.bookingtime {GetSearchDirection(searchOptions.IsDescending)} ";
                    queryJoins.Add(startBookingsJoin);
                    break;
                default:
                    queryOrder =
                        $@"ORDER BY {bookingsTable}.bookingtime {GetSearchDirection(searchOptions.IsDescending)} ";
                    queryJoins.Add(startBookingsJoin);
                    break;
            }

            if (queryJoins.Count > 0)
                query += string.Join(" ", queryJoins.ToHashSet());
            if (queryFilter.Count > 0)
                query += "WHERE " + string.Join("AND ", queryFilter);

            query += queryOrder;
            query += @"), TempCount AS (
                        SELECT COUNT(*) AS TotalRowCount FROM TempResult
                        ) 
                    SELECT * FROM TempResult as data, TempCount AS meta
                    Offset @skip Rows
                    Fetch Next @take Rows Only ";

            DapperExtensions.DapperExtensions.SqlDialect = new PostgreSqlDialect();

            var totalCount = (Int64)0;
            using var con = new NpgsqlConnection(ConnectionString);
            con.Open();
            var res = con.Query<DutyHours>(
                query,
                new[]
                {
                    typeof(DbDutyHours), typeof(Int64)
                },
                (objects) =>
                {
                    var res = (DbDutyHours)objects[0];
                    totalCount = (Int64)objects[1];
                    return Transformer.ToEntity(res);
                },
                queryParams,
                splitOn: "ident,TotalRowCount"
            ).ToList();
            return new DataTableSearchResult<DutyHours>()
            {
                Data = res,
                TotalRowCount = totalCount
            };
        }

        private string GetSearchDirection(bool? isDescending) => isDescending == true ? "DESC" : "ASC";
    }
}