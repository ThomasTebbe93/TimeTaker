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
    public class DutyHoursBookingController : DefaultController
    {

        private readonly IDutyHoursBookingService dutyHoursBookingService;

        public DutyHoursBookingController(
            IDutyHoursBookingService dutyHoursBookingService, 
            IRequestService requestService) : base(
            requestService)
        {
            this.dutyHoursBookingService = dutyHoursBookingService;
        }
        
        [HttpGet("getPersonAndStateByChipId/{chipId}")]
        [ActionName("JSONMethod")]
        public IActionResult GetPersonAndStateByChipId(string chipId)
        {
            var res = dutyHoursBookingService.GetPersonAndStateByChipId(Context, chipId);

            return Ok(res);
        }
        
        [HttpPost("signIn")]
        [ActionName("JSONMethod")]
        public IActionResult SignIn()
        {
            var res = dutyHoursBookingService.SignIn(Context);

            return Ok(res);
        }
        
        [HttpPost("signInByChip")]
        [ActionName("JSONMethod")]
        public IActionResult SignInByChip(ChipSingEntity entity)
        {
            var res = dutyHoursBookingService.SignInByChip(Context, entity);

            return Ok(res);
        }
        
        [HttpPost("signOut")]
        [ActionName("JSONMethod")]
        public IActionResult SignOut()
        {
            var res = dutyHoursBookingService.SignOut(Context);

            return Ok(res);
        }
        
        [HttpPost("signOutByChip")]
        [ActionName("JSONMethod")]
        public IActionResult SignOutByChip(ChipSingEntity entity)
        {
            var res = dutyHoursBookingService.SignOutByChip(Context, entity);

            return Ok(res);
        }

        [HttpGet("getPersonalBookings")]
        [ActionName("JSONMethod")]
        public IActionResult GetPersonalBookings()
        {
            if (!Context.User.Role.Rights.Select(x => x.Key).ToHashSet()
                    .Contains(Rights.DutyHoursDisplaySelf))
                return Ok(new RequestResult()
                {
                    PermissionFailure = new PermissionFailure()
                    {
                        FailureMessage = PermissionFailureMessage.MissingPermission,
                        UnderlyingRight = Rights.DutyHoursDisplaySelf
                    },
                    StatusCode = Base.StatusCode.PermissionFailure
                });
            var res = dutyHoursBookingService.GetPersonalBookings(Context);

            return Ok(res);
        }
    }
}