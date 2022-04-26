using System;
using System.Collections.Generic;
using System.Linq;
using API.BLL.Base;
using API.BLL.UseCases.DutyHoursManagement.Entities;
using API.BLL.UseCases.DutyHoursManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.BLL.UseCases.DutyHoursManagement.Controller
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DutyHoursController : DefaultController
    {

        private readonly IDutyHoursService dutyHoursService;
        private readonly IDutyHoursBookingService dutyHoursBookingService;

        public DutyHoursController(
            IDutyHoursService dutyHoursService, 
            IDutyHoursBookingService dutyHoursBookingService, 
            IRequestService requestService) : base(
            requestService)
        {
            this.dutyHoursService = dutyHoursService;
            this.dutyHoursBookingService = dutyHoursBookingService;
        }
        
        [HttpPost("findBySearchValue")]
        public IActionResult FindBySearchValue(DutyHoursSearchOptions searchOptions)
        {
            if (!Context.User.Role.Rights.Select(x => x.Key).ToHashSet()
                    .Contains(Rights.DutyHours))
                return Ok(new RequestResult()
                {
                    PermissionFailure = new PermissionFailure()
                    {
                        FailureMessage = PermissionFailureMessage.MissingPermission,
                        UnderlyingRight = Rights.DutyHours
                    },
                    StatusCode = Base.StatusCode.PermissionFailure
                });
            var res = dutyHoursService.FindBySearchValue(Context, searchOptions);

            return Ok(res);
        }
        
        [HttpGet("getPersonalHoursWithState")]
        [ActionName("JSONMethod")]
        public IActionResult GetPersonalHoursWithState()
        {
            if (!Context.User.Role.Rights.Select(x => x.Key).ToHashSet()
                    .Contains(Rights.DutyHours))
                return Ok(new RequestResult()
                {
                    PermissionFailure = new PermissionFailure()
                    {
                        FailureMessage = PermissionFailureMessage.MissingPermission,
                        UnderlyingRight = Rights.DutyHours
                    },
                    StatusCode = Base.StatusCode.PermissionFailure
                });
            var hours = dutyHoursService.GetPersonalHours(Context);
            var lastBooking = dutyHoursBookingService.GetLastBooking(Context);

            var res = new DutyHoursState
            {
                Hours = hours,
                Booking = lastBooking
            };
            
            return Ok(res);
        }
        
        [HttpGet("getByIdent/{ident}")]
        public IActionResult GetByIdent(Guid ident)
        {
            if (!Context.User.Role.Rights.Select(x => x.Key).ToHashSet()
                    .Contains(Rights.DutyHoursEditBooking))
                return Ok(new RequestResult()
                {
                    PermissionFailure = new PermissionFailure()
                    {
                        FailureMessage = PermissionFailureMessage.MissingPermission,
                        UnderlyingRight = Rights.DutyHoursEditBooking
                    },
                    StatusCode = Base.StatusCode.PermissionFailure
                });
            var vehicle = dutyHoursService.FindByIdent(new DutyHoursIdent(ident));

            return Ok(vehicle);
        }
        
        [HttpPost("update")]
        [ActionName("JSONMethod")]
        public IActionResult Update(List<DutyHoursRestEntity> dutyHours)
        {
            var res = dutyHoursService.Update(Context, dutyHours);

            return Ok(res);
        }
    }
}