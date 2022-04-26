using System.Collections.Generic;
using System.Linq;
using API.BLL.Helper;
using API.BLL.UseCases.DutyHoursManagement.Daos;
using API.BLL.UseCases.DutyHoursManagement.Entities;
using API.BLL.UseCases.DutyHoursManagement.Transformer;
using API.BLL.UseCases.Memberships.Entities;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

namespace API.DAL.UseCases.DutyHoursManagement
{
    public class DutyHoursBookingDao :
        AbstractPostgresqlDao<DutyHoursBooking, DbDutyHoursBooking, DutyHoursBookingIdent, DutyHoursBookingTransformer>,
        IDutyHoursBookingDao
    {
        public DutyHoursBookingDao(IOptions<AppSettings> appSettings) : base(appSettings, "dutyhoursbookings")
        {
        }

        public DutyHoursBooking FindLastByUser( UserIdent userIdent)
        {
            using var con = new NpgsqlConnection(ConnectionString);
            con.Open();

            var res = con.QueryFirstOrDefault<DbDutyHoursBooking>(
                $@"
                        SELECT *
                        FROM {TableName}
                        WHERE userident = @userident
                        ORDER BY bookingtime DESC
                        Fetch Next 1 Rows Only ",
                new
                {
                    userident = userIdent.Ident,
                }
            );

            return res == null
                ? null
                : Transformer.ToEntity(res);
        }

        public List<DutyHoursBooking> GetPersonalBookings(Context context)
        {
            using var con = new NpgsqlConnection(ConnectionString);
            con.Open();

            return con.Query<DbDutyHoursBooking>(
                $@"
                        SELECT *
                        FROM {TableName}
                        WHERE userident = @userident
                        ORDER BY bookingtime DESC",
                new
                {
                    userident = context.User.Ident.Ident,
                }
            ).Select(x => Transformer.ToEntity(x)).ToList();
        }
    }
}