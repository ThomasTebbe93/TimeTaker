using System.Threading.Tasks;
using API.BLL.Base;
using API.BLL.UseCases.DrkServerConnector.Entities;
using API.BLL.UseCases.DrkServerConnector.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.BLL.UseCases.DrkServerConnector.Controller
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DrkServerConnectorController : DefaultController
    {
        private readonly IDrkServerImportService importService;
        
        public DrkServerConnectorController( 
            IDrkServerImportService importService,
            IRequestService requestService) : base(requestService)
        {
            this.importService = importService;
        }
        
        [HttpPost("import")]
        [ActionName("JSONMethod")]
        public async Task<IActionResult> Import(ImportRequest importRequest)
        {
            var res = await importService.Import(Context, importRequest);

            return Ok(res);
        }
    }
}