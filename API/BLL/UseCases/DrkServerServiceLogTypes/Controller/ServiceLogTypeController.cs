using System.Linq;
using API.BLL.Base;
using API.BLL.UseCases.DrkServerServiceLogTypes.Entities;
using API.BLL.UseCases.DrkServerServiceLogTypes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.BLL.UseCases.DrkServerServiceLogTypes.Controller
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ServiceLogTypeController : DefaultController
        {
        private readonly IServiceLogTypeService typeService;
        
        public ServiceLogTypeController( 
            IServiceLogTypeService typeService,
            IRequestService requestService) : base(requestService)
        {
            this.typeService = typeService;
        }
        
        
        [HttpPost("findBySearchValue")]
        public IActionResult FindBySearchValue(ServiceLogTypeSearchOptions searchOptions)
        {
            if (!Context.User.Role.Rights.Select(x => x.Key).ToHashSet()
                    .Contains(Rights.Administration))
                Ok(new RequestResult()
                {
                    PermissionFailure = new PermissionFailure()
                    {
                        FailureMessage = PermissionFailureMessage.MissingPermission,
                        UnderlyingRight = Rights.Administration
                    },
                    StatusCode = Base.StatusCode.PermissionFailure
                });
        
            var users = typeService.FindBySearchValue(searchOptions);
            return Ok(users);
        }
    }
}