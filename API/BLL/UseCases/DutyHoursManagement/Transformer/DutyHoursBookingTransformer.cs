using API.BLL.Base;
using API.BLL.Extensions;
using API.BLL.UseCases.DutyHoursManagement.Entities;
using API.BLL.UseCases.Memberships.Entities;
using API.DAL.UseCases.DutyHoursManagement;

namespace API.BLL.UseCases.DutyHoursManagement.Transformer
{
    public class DutyHoursBookingTransformer : ITransformer<DutyHoursBooking, DbDutyHoursBooking>
    {
        public DutyHoursBooking ToEntity(DbDutyHoursBooking entity)
        {
            return new DutyHoursBooking()
            {
                Ident = entity.Ident.Ident<DutyHoursBookingIdent>(),
                UserIdent = entity.UserIdent.Ident<UserIdent>(),
                CreatorIdent = entity.CreatorIdent.Ident<UserIdent>(),
                BookingTime = entity.BookingTime,
                IsSignedIn = entity.IsSignedIn,
            };
        }

        public DbDutyHoursBooking ToDbEntity(DutyHoursBooking entity)
        {
            return new DbDutyHoursBooking()
            {
                Ident = entity.Ident.Ident,
                UserIdent = entity.UserIdent.Ident,
                CreatorIdent = entity.CreatorIdent.Ident,
                BookingTime = entity.BookingTime,
                IsSignedIn = entity.IsSignedIn,
            };
        }
    }
}