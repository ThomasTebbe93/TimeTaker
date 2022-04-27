using System.Collections.Generic;
using System.Linq;
using API.BLL.Extensions;
using API.BLL.Helper;
using API.BLL.UseCases.DrkServerServiceLogDescriptions.Daos;
using API.BLL.UseCases.DrkServerServiceLogTypes.Daos;
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
        private readonly IServiceLogTypeDao typeDao;
        private readonly IServiceLogDescriptionDao descriptionDao;

        public DutyHoursEnricher(
            IDutyHoursBookingDao dutyHoursBookingDao,
            IServiceLogTypeDao typeDao,
            IServiceLogDescriptionDao descriptionDao,
            IUserDao userDao)
        {
            this.dutyHoursBookingDao = dutyHoursBookingDao;
            this.typeDao = typeDao;
            this.descriptionDao = descriptionDao;
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

            var serviceLogTypeIds = hours.Where(x => x.ServiceLogTypeId.HasValue).Select(x => x.ServiceLogTypeId.Value)
                .ToList();
            var serviceLogTypes = typeDao.FindByIds(serviceLogTypeIds.ToHashSet()).ToDictionary(x => x.Id);
            var serviceLogDescriptionIds = hours.Where(x => x.ServiceLogDescriptionId.HasValue)
                .Select(x => x.ServiceLogDescriptionId.Value).ToList();
            var serviceLogDescriptions =
                descriptionDao.FindByIds(serviceLogDescriptionIds.ToHashSet()).ToDictionary(x => x.Id);


            var enrichedBookings = bookings.Select(x => new DutyHoursBooking(x)
            {
                Creator = users.ValueOrDefault(x.CreatorIdent),
                User = users.ValueOrDefault(x.UserIdent)
            }).ToDictionary(x => x.Ident);

            return hours.Select(x => new DutyHours(x)
            {
                SignInBooking = enrichedBookings.ValueOrDefault(x.SignInBookingIdent),
                SignOutBooking = enrichedBookings.ValueOrDefault(x.SignOutBookingIdent),
                ServiceLogType = x.ServiceLogTypeId.HasValue
                    ? serviceLogTypes.ValueOrDefault(x.ServiceLogTypeId.Value)
                    : null,
                ServiceLogDescription = x.ServiceLogDescriptionId.HasValue
                    ? serviceLogDescriptions.ValueOrDefault(x.ServiceLogDescriptionId.Value)
                    : null
            }).ToList();
        }

        public DutyHours Enrich(DutyHours hour)
        {
            var bookingIdents = new HashSet<DutyHoursBookingIdent>()
                { hour.SignInBookingIdent, hour.SignOutBookingIdent };
            var bookings = dutyHoursBookingDao.FindByIdents(bookingIdents);

            var creatorIdents = bookings.Select(x => x.CreatorIdent);
            var userIdents = bookings.Select(x => x.UserIdent);

            var personIdents = userIdents.Concat(creatorIdents).ToHashSet();
            var users = userDao.FindByIdents(personIdents)
                .Select(x => x.ToOutputUser())
                .ToDictionary(x => x.Ident);

            var serviceLogTypeIds = new HashSet<int>();
            if (hour.ServiceLogTypeId.HasValue)
                serviceLogTypeIds.Add(hour.ServiceLogTypeId.Value);
            var serviceLogTypes = typeDao.FindByIds(serviceLogTypeIds.ToHashSet()).ToDictionary(x => x.Id);
            var serviceLogDescriptionIds = new HashSet<int>();
            if (hour.ServiceLogDescriptionId.HasValue)
                serviceLogDescriptionIds.Add(hour.ServiceLogDescriptionId.Value);
            var serviceLogDescriptions =
                descriptionDao.FindByIds(serviceLogDescriptionIds.ToHashSet()).ToDictionary(x => x.Id);

            var enrichedBookings = bookings.Select(x => new DutyHoursBooking(x)
            {
                Creator = users.ValueOrDefault(x.CreatorIdent),
                User = users.ValueOrDefault(x.UserIdent)
            }).ToDictionary(x => x.Ident);

            return new DutyHours(hour)
            {
                SignInBooking = enrichedBookings.ValueOrDefault(hour.SignInBookingIdent),
                SignOutBooking = enrichedBookings.ValueOrDefault(hour.SignOutBookingIdent),
                ServiceLogType = hour.ServiceLogTypeId.HasValue
                    ? serviceLogTypes.ValueOrDefault(hour.ServiceLogTypeId.Value)
                    : null,
                ServiceLogDescription = hour.ServiceLogDescriptionId.HasValue
                    ? serviceLogDescriptions.ValueOrDefault(hour.ServiceLogDescriptionId.Value)
                    : null
            };
        }
    }
}