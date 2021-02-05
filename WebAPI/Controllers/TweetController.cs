using System.Linq;
using System.Threading.Tasks;
using Core.Processing;
using Core.Processing.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebAPI.Helpers;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]    
    public class TweetController : Controller
    {
        private ILogger<TweetController> Logger { get; }
        private IConfiguration Configuration { get; }
        private TweetAnalysisService TweetAnalysisService { get; }

        public TweetController(
            ILogger<TweetController> logger,
            IConfiguration configuration,
            IHostedServiceAccessor<TweetAnalysisService> accessor)
        {
            Logger = logger;
            Configuration = configuration;
            TweetAnalysisService = accessor.Service;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = TweetAnalysisService.Analysis;
            return Ok(data);
        }
    }
}