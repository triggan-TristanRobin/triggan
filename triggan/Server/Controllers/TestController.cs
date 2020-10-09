using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Configuration;

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
            return Configuration.GetSection("Test").GetSection("inner").Value;
        }

        [HttpGet("[action]")]
        public string Get()
        {
            return "Hello there";
        }
    }
}
