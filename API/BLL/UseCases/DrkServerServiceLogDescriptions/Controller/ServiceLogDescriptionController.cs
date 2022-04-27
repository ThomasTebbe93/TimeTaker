using System.Linq;
using System.Threading.Tasks;
using API.BLL.Base;
using API.BLL.UseCases.DrkServerConnector.Entities;
using API.BLL.UseCases.DrkServerConnector.Services;
using API.BLL.UseCases.DrkServerServiceLogDescriptions.Entities;
using API.BLL.UseCases.DrkServerServiceLogDescriptions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.BLL.UseCases.DrkServerServiceLogDescriptions.Controller
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ServiceLogDescriptionController : DefaultController
    {
        private readonly IServiceLogDescriptionService descriptionService;
        
        public ServiceLogDescriptionController( 
            IServiceLogDescriptionService descriptionService,
            IRequestService requestService) : base(requestService)
        {
            this.descriptionService = descriptionService;
        }
        
        [HttpPost("autocomplete")]
        [ActionName("JSONMethod")]
        public IActionResult AutoComplete(AutoCompleteOptions autoCompleteOptions)
        {
            var res = descriptionService.Autocomplete(autoCompleteOptions.SearchValue);
            return Ok(res);
        }
        
        [HttpPost("findBySearchValue")]
        public IActionResult FindBySearchValue(ServiceLogDescriptionSearchOptions searchOptions)
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
        
            var users = descriptionService.FindBySearchValue(searchOptions);
            return Ok(users);
        }
    }
}