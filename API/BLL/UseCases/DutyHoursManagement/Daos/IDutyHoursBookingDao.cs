using System.Collections.Generic;
using API.BLL.Base;
using API.BLL.Helper;
using API.BLL.UseCases.DutyHoursManagement.Entities;
using API.BLL.UseCases.DutyHoursManagement.Transformer;
using API.BLL.UseCases.Memberships.Entities;

namespace API.BLL.UseCases.DutyHoursManagement.Daos
{
    public interface IDutyHoursBookingDao : IDao<DutyHoursBooking, DutyHoursBookingIdent, DutyHoursBookingTransformer>
    {
        public DutyHoursBooking FindLastByUser(UserIdent userIdent);
        public List<DutyHoursBooking> GetPersonalBookings(Context context);
    }
}