using AutoMapper.Configuration;
using Core.Processing.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]    
    public class TweetController : Controller
    {
        private ILogger<TweetController> Logger { get; }
        private IConfiguration Configuration { get; }
        private ITweetAnalysisService TweetAnalysisService { get; }

        public TweetController(
            ILogger<TweetController> logger,
            IConfiguration configuration,
            ITweetAnalysisService tweetAnalysisService)
        {
            Logger = logger;
            Configuration = configuration;
            TweetAnalysisService = tweetAnalysisService;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}