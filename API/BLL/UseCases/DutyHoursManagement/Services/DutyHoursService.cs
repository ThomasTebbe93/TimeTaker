using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using API.BLL.Base;
using API.BLL.Extensions;
using API.BLL.Helper;
using API.BLL.UseCases.DutyHoursManagement.Daos;
using API.BLL.UseCases.DutyHoursManagement.Entities;
using API.BLL.UseCases.DutyHoursManagement.Validation;

namespace API.BLL.UseCases.DutyHoursManagement.Services
{
    public interface IDutyHoursService
    {
        DataTableSearchResult<DutyHours> FindBySearchValue(Context context, DutyHoursSearchOptions search);
        List<DutyHours> GetPersonalHours(Context context);
        DutyHours FindByIdent(DutyHoursIdent ident);
        RequestResult Update(Context context, List<DutyHoursRestEntity> dutyHours);
    }

    public class DutyHoursService : IDutyHoursService
    {
        private readonly IDutyHoursDao dutyHoursDao;
        private readonly IDutyHoursBookingDao dutyHoursBookingDao;
        private readonly DutyHoursEnricher enricher;

        public DutyHoursService(
            IDutyHoursDao dutyHoursDao,
            IDutyHoursBookingDao dutyHoursBookingDao,
            DutyHoursEnricher enricher)
        {
            this.dutyHoursDao = dutyHoursDao;
            this.dutyHoursBookingDao = dutyHoursBookingDao;
            this.enricher = enricher;
        }

        public DutyHours FindByIdent(DutyHoursIdent ident)
        {
            var res = dutyHoursDao.FindByIdent(ident);
            return enricher.Enrich(res);
        }

        public RequestResult Update(Context context, List<DutyHoursRestEntity> dutyHours)
        {
            var validator = new DutyHoursRestEntityValidator();
            try
            {
                var results = dutyHours.Select(dutyHour => validator.Validate(dutyHour));
                var validationFailures = results.SelectMany(x => x.Errors).ToList();
                if (validationFailures.Any())
                    return new RequestResult()
                    {
                        ValidationFailures = validationFailures,
                        StatusCode = StatusCode.ValidationError
                    };

                var oldDutyHours = dutyHoursDao.FindByIdents(
                    dutyHours.Where(x => x.Ident != null)
                        .Select(dutyHour => new DutyHoursIdent((Guid)dutyHour.Ident))
                        .ToHashSet());
                var enrichedOldDutyHours = enricher.Enrich(oldDutyHours).ToDictionary(x => x.Ident);

                var toUpdateDutyHours = dutyHours.Select(dutyHour =>
                {
                    var existing = enrichedOldDutyHours.ValueOrDefault(dutyHour.Ident.IdentOrNull<DutyHoursIdent>());
                    return new DutyHours(existing)
                    {
                        SignInBooking = new DutyHoursBooking(existing.SignInBooking)
                        {
                            BookingTime = dutyHour.Start
                        },
                        SignOutBooking = new DutyHoursBooking(existing.SignOutBooking)
                        {
                            BookingTime = dutyHour.End
                        },
                        ServiceLogTypeId = dutyHour.ServiceLogTypeId ?? existing.ServiceLogTypeId,
                        ServiceLogDescriptionId = dutyHour.ServiceLogDescriptionId ?? existing.ServiceLogDescriptionId
                    };
                }).ToList();

                var toUpdateBookings = toUpdateDutyHours.SelectMany(dutyHour => new List<DutyHoursBooking>
                {
                    dutyHour.SignInBooking,
                    dutyHour.SignOutBooking
                }).ToList();

                if (!context.User.Role.Rights.Select(x => x.Key).ToHashSet()
                        .Contains(Rights.DutyHoursEditBooking) && toUpdateDutyHours.Count > 0)
                    return new RequestResult()
                    {
                        PermissionFailure = new PermissionFailure()
                        {
                            FailureMessage = PermissionFailureMessage.MissingPermission,
                            UnderlyingRight = Rights.DutyHoursEditBooking
                        },
                        StatusCode = StatusCode.PermissionFailure
                    };
                using var transactionScope = new TransactionScope();

                dutyHoursDao.UpdateMany(toUpdateDutyHours);
                dutyHoursBookingDao.UpdateMany(toUpdateBookings);

                transactionScope.Complete();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new RequestResult()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Exception = e
                };
            }

            return new RequestResult()
            {
                StatusCode = StatusCode.Ok
            };
        }

        public DataTableSearchResult<DutyHours> FindBySearchValue(Context context, DutyHoursSearchOptions search)
        {
            var dataTableSearchResult = dutyHoursDao.FindBySearchValue(context, search);
            var enriched = enricher.Enrich(dataTableSearchResult.Data);

            return new DataTableSearchResult<DutyHours>
            {
                Data = enriched,
                TotalRowCount = dataTableSearchResult.TotalRowCount
            };
        }

        public List<DutyHours> GetPersonalHours(Context context)
        {
            var hours = dutyHoursDao.GetPersonalBookings(context);
            var enriched = enricher.Enrich(hours);

            return enriched;
        }
    }
}