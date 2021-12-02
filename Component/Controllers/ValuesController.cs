using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Component.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Get", "value1" };
        }

        [HttpPost]
        [Authorize("write")]
        public ActionResult<IEnumerable<string>> Post()
        {
            return new string[] { "Post", "value2" };
        }

        [HttpDelete]
        [Authorize("delete")]
        public ActionResult<IEnumerable<string>> Delete()
        {
            return new string[] { "Delete", "value3" };
        }
    }
}
