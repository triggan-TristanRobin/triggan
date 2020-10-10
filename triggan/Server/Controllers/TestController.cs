using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Diagnostics;

namespace triggan.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        public IConfiguration Configuration { get; }

        public TestController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet("[action]")]
        public string GetConfig()
        {
            Trace.TraceInformation("Calling get config from test");
            return Configuration.GetSection("Test").GetSection("inner").Value;
        }

        [HttpGet("[action]")]
        public string Get()
        {
            Trace.TraceInformation("Calling get from test");
            return "Hello there";
        }
    }
}
