using System.Collections.Generic;
using System.Linq;
using API.BLL.Extensions;
using API.BLL.Helper;
using API.BLL.UseCases.DutyHoursManagement.Daos;
using API.BLL.UseCases.DutyHoursManagement.Entities;
using API.BLL.UseCases.Memberships.Daos;
using API.BLL.UseCases.Memberships.Entities;

namespace API.BLL.UseCases.DutyHoursManagement.Services
{
    public class DutyHoursEnricher
    {
        private readonly IDutyHoursBookingDao dutyHoursBookingDao;
        private readonly IUserDao userDao;

        public DutyHoursEnricher(
            IDutyHoursBookingDao dutyHoursBookingDao,
            IUserDao userDao)
        {
            this.dutyHoursBookingDao = dutyHoursBookingDao;
            this.userDao = userDao;
        }

        public List<DutyHours> Enrich(List<DutyHours> hours)
        {
            var singInBookingIdents = hours.Select(x => x.SignInBookingIdent).ToList();
            var signOutBookingIdents = hours.Select(x => x.SignOutBookingIdent).ToList();
            var bookingIdents = singInBookingIdents.Concat(signOutBookingIdents).ToHashSet();
            var bookings = dutyHoursBookingDao.FindByIdents(bookingIdents);

            var creatorIdents = bookings.Select(x => x.CreatorIdent);
            var userIdents = bookings.Select(x => x.UserIdent);

            var personIdents = userIdents.Concat(creatorIdents).ToHashSet();
            var users = userDao.FindByIdents(personIdents)
                .Select(x => x.ToOutputUser())
                .ToDictionary(x => x.Ident);

            var enrichedBookings = bookings.Select(x => new DutyHoursBooking(x)
            {
                Creator = users.ValueOrDefault(x.CreatorIdent),
                User = users.ValueOrDefault(x.UserIdent)
            }).ToDictionary(x => x.Ident);

            return hours.Select(x => new DutyHours(x)
                {
                    SignInBooking = enrichedBookings.ValueOrDefault(x.SignInBookingIdent),
                    SignOutBooking = enrichedBookings.ValueOrDefault(x.SignOutBookingIdent)
                }).ToList();
        }
        public DutyHours Enrich(DutyHours hour)
        {
            var bookingIdents = new HashSet<DutyHoursBookingIdent>(){hour.SignInBookingIdent, hour.SignOutBookingIdent};
            var bookings = dutyHoursBookingDao.FindByIdents(bookingIdents);

            var creatorIdents = bookings.Select(x => x.CreatorIdent);
            var userIdents = bookings.Select(x => x.UserIdent);

            var personIdents = userIdents.Concat(creatorIdents).ToHashSet();
            var users = userDao.FindByIdents(personIdents)
                .Select(x => x.ToOutputUser())
                .ToDictionary(x => x.Ident);

            var enrichedBookings = bookings.Select(x => new DutyHoursBooking(x)
            {
                Creator = users.ValueOrDefault(x.CreatorIdent),
                User = users.ValueOrDefault(x.UserIdent)
            }).ToDictionary(x => x.Ident);

            return new DutyHours(hour)
                {
                    SignInBooking = enrichedBookings.ValueOrDefault(hour.SignInBookingIdent),
                    SignOutBooking = enrichedBookings.ValueOrDefault(hour.SignOutBookingIdent)
                };
        }
    }
}