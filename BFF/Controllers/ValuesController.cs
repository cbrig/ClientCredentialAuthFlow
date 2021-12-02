using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BFF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class ValuesController : ControllerBase
    {
        private readonly IComponentService _componentService;

        public ValuesController(IComponentService componentService)
        {
            _componentService = componentService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var values = await _componentService.GetValues();
                return new JsonResult(values) { StatusCode = 200 }; 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
