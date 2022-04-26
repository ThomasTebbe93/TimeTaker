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
using API.BLL.UseCases.Memberships.Daos;
using API.BLL.UseCases.Memberships.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.BLL.UseCases.DutyHoursManagement.Services
{
    public interface IDutyHoursBookingService
    {
        public RequestResult SignIn(Context context);
        public RequestResult SignInByChip(Context context, ChipSingEntity input);
        public RequestResult SignOut(Context context);
        public RequestResult SignOutByChip(Context context, ChipSingEntity input);
        public List<DutyHoursBooking> GetPersonalBookings(Context context);
        public DutyHoursBooking GetLastBooking(Context context);
        public IActionResult GetPersonAndStateByChipId(Context context, string chipId);
    }

    public class DutyHoursBookingService : IDutyHoursBookingService
    {
        private readonly IDutyHoursBookingDao dutyHoursBookingDao;
        private readonly IDutyHoursDao dutyHoursDao;
        private readonly IUserDao userDao;

        public DutyHoursBookingService(
            IDutyHoursBookingDao dutyHoursBookingDao,
            IDutyHoursDao dutyHoursDao,
            IUserDao userDao)
        {
            this.dutyHoursBookingDao = dutyHoursBookingDao;
            this.dutyHoursDao = dutyHoursDao;
            this.userDao = userDao;
        }

        public RequestResult SignIn(Context context)
        {
            try
            {
                if (!context.User.Role.Rights.Select(x => x.Key).ToHashSet()
                        .Contains(Rights.DutyHoursCreateBooking))
                    return new RequestResult()
                    {
                        PermissionFailure = new PermissionFailure()
                        {
                            FailureMessage = PermissionFailureMessage.MissingPermission,
                            UnderlyingRight = Rights.DutyHoursCreateBooking
                        },
                        StatusCode = StatusCode.PermissionFailure
                    };

                var toCreate = new DutyHoursBooking()
                {
                    Ident = Guid.NewGuid().Ident<DutyHoursBookingIdent>(),
                    UserIdent = context.User.Ident,
                    CreatorIdent = context.User.Ident,
                    BookingTime = DateTimeOffset.Now,
                    IsSignedIn = true
                };

                var lastBooking = dutyHoursBookingDao.FindLastByUser(context.User.Ident);
                var validator = new DutyHoursBookingValidator(lastBooking);

                var result = validator.Validate(toCreate);
                var validationFailures = result.Errors.ToList();
                if (validationFailures.Any())
                    return new RequestResult()
                    {
                        ValidationFailures = validationFailures,
                        StatusCode = StatusCode.ValidationError
                    };

                dutyHoursBookingDao.Create(toCreate);
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

        public RequestResult SignInByChip(Context context, ChipSingEntity input)
        {
            try
            {
                if (!context.User.Role.Rights.Select(x => x.Key).ToHashSet()
                        .Contains(Rights.DutyHoursCreateBooking))
                    return new RequestResult()
                    {
                        PermissionFailure = new PermissionFailure()
                        {
                            FailureMessage = PermissionFailureMessage.MissingPermission,
                            UnderlyingRight = Rights.DutyHoursCreateBooking
                        },
                        StatusCode = StatusCode.PermissionFailure
                    };

                var user = userDao.FindByIdent(input.UserIdent.ToIdent<UserIdent>());

                var toCreate = new DutyHoursBooking()
                {
                    Ident = Guid.NewGuid().Ident<DutyHoursBookingIdent>(),
                    UserIdent = user.Ident,
                    CreatorIdent = user.Ident,
                    BookingTime = DateTimeOffset.Now,
                    IsSignedIn = true
                };

                var lastBooking = dutyHoursBookingDao.FindLastByUser(user.Ident);
                var validator = new DutyHoursBookingValidator(lastBooking);

                var result = validator.Validate(toCreate);
                var validationFailures = result.Errors.ToList();
                if (validationFailures.Any())
                    return new RequestResult()
                    {
                        ValidationFailures = validationFailures,
                        StatusCode = StatusCode.ValidationError
                    };

                dutyHoursBookingDao.Create(toCreate);
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

        public RequestResult SignOut(Context context)
        {
            try
            {
                if (!context.User.Role.Rights.Select(x => x.Key).ToHashSet()
                        .Contains(Rights.DutyHoursCreateBooking))
                    return new RequestResult()
                    {
                        PermissionFailure = new PermissionFailure()
                        {
                            FailureMessage = PermissionFailureMessage.MissingPermission,
                            UnderlyingRight = Rights.DutyHoursCreateBooking
                        },
                        StatusCode = StatusCode.PermissionFailure
                    };

                var toCreate = new DutyHoursBooking()
                {
                    Ident = Guid.NewGuid().Ident<DutyHoursBookingIdent>(),
                    UserIdent = context.User.Ident,
                    CreatorIdent = context.User.Ident,
                    BookingTime = DateTimeOffset.Now,
                    IsSignedIn = false
                };

                var lastBooking = dutyHoursBookingDao.FindLastByUser(context.User.Ident);
                var validator = new DutyHoursBookingValidator(lastBooking);

                var result = validator.Validate(toCreate);
                var validationFailures = result.Errors.ToList();
                if (validationFailures.Any())
                    return new RequestResult()
                    {
                        ValidationFailures = validationFailures,
                        StatusCode = StatusCode.ValidationError
                    };


                var dutyHoursToCreate = new DutyHours()
                {
                    Ident = Guid.NewGuid().Ident<DutyHoursIdent>(),
                    SignInBookingIdent = lastBooking.Ident,
                    SignOutBookingIdent = toCreate.Ident
                };
                using var transactionScope = new TransactionScope();

                dutyHoursBookingDao.Create(toCreate);
                dutyHoursDao.Create(dutyHoursToCreate);

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


        public RequestResult SignOutByChip(Context context, ChipSingEntity input)
        {
            try
            {
                if (!context.User.Role.Rights.Select(x => x.Key).ToHashSet()
                        .Contains(Rights.DutyHoursCreateBooking))
                    return new RequestResult()
                    {
                        PermissionFailure = new PermissionFailure()
                        {
                            FailureMessage = PermissionFailureMessage.MissingPermission,
                            UnderlyingRight = Rights.DutyHoursCreateBooking
                        },
                        StatusCode = StatusCode.PermissionFailure
                    };

                var user = userDao.FindByIdent( input.UserIdent.ToIdent<UserIdent>());

                var toCreate = new DutyHoursBooking()
                {
                    Ident = Guid.NewGuid().Ident<DutyHoursBookingIdent>(),
                    UserIdent = user.Ident,
                    CreatorIdent = user.Ident,
                    BookingTime = DateTimeOffset.Now,
                    IsSignedIn = false
                };

                var lastBooking = dutyHoursBookingDao.FindLastByUser( user.Ident);
                var validator = new DutyHoursBookingValidator(lastBooking);

                var result = validator.Validate(toCreate);
                var validationFailures = result.Errors.ToList();
                if (validationFailures.Any())
                    return new RequestResult()
                    {
                        ValidationFailures = validationFailures,
                        StatusCode = StatusCode.ValidationError
                    };


                var dutyHoursToCreate = new DutyHours()
                {
                    Ident = Guid.NewGuid().Ident<DutyHoursIdent>(),
                    SignInBookingIdent = lastBooking.Ident,
                    SignOutBookingIdent = toCreate.Ident
                };
                using var transactionScope = new TransactionScope();

                dutyHoursBookingDao.Create(toCreate);
                dutyHoursDao.Create(dutyHoursToCreate);

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

        public List<DutyHoursBooking> GetPersonalBookings(Context context) =>
            dutyHoursBookingDao.GetPersonalBookings(context);

        public DutyHoursBooking GetLastBooking(Context context) =>
            dutyHoursBookingDao.FindLastByUser(context.User.Ident);

        public IActionResult GetPersonAndStateByChipId(Context context, string chipId)
        {
            if (!context.User.Role.Rights.Select(x => x.Key).ToHashSet()
                    .Contains(Rights.DutyHoursGetUserByChipId))
                return new OkObjectResult(new RequestResult()
                {
                    PermissionFailure = new PermissionFailure()
                    {
                        FailureMessage = PermissionFailureMessage.MissingPermission,
                        UnderlyingRight = Rights.DutyHours
                    },
                    StatusCode = StatusCode.PermissionFailure
                });

            var user = userDao.FindByChipId(chipId);
            if (user == null )
            {
                return new OkObjectResult(new RequestResult()
                {
                    Exception = new Exception($"no matching User for ChipId {chipId}"),
                    StatusCode = StatusCode.ValidationError
                });
            }

            var lastBooking = dutyHoursBookingDao.FindLastByUser(user.Ident);

            var res = new PersonAndStateByChipIdResult
            {
                LastBooking = lastBooking,
                User = user.ToOutputUser()
            };
            return new OkObjectResult(res);
        }
    }
}