using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiagnosticController : Controller
    {
        private ILogger<DiagnosticController> Logger { get; }
        private IConfiguration Configuration { get; }

        public DiagnosticController(
            ILogger<DiagnosticController> logger,
            IConfiguration configuration)
        {
            Logger = logger;
            Configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                ApiKey = Configuration.GetValue<string>("TwitterApi:ApiKey"),
                BearerToken = Configuration.GetValue<string>("TwitterApi:BearerToken"),
                ApiUrl = Configuration.GetValue<string>("TwitterApi:ApiUrl"),
                StreamIntervalSeconds = Configuration.GetValue<int>("TwitterApi:StreamIntervalSeconds")
            });
        }
    }
}