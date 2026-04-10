using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace BnbApi.Controllers
{
    [ApiController]
    [Produces("application/xml")]
    [Route("api/[controller]/[action]")]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/Get/5
        [HttpGet]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

    }
}
