using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Data.Interfaces;
using Core.Processing.Interfaces;
using Core.Processing.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Processing
{
    public class TweetAnalysisService : BackgroundService, ITweetAnalysisService
    {
        private ILogger<TweetAnalysisService> Logger { get; }
        private IConfiguration Configuration { get; }
        private ITweetRepository TweetRepository { get; }

        private Mapper TweetMapper { get; set; }

        public TweetAnalysis Analysis { get; private set; }

        public TweetAnalysisService(
            ILogger<TweetAnalysisService> logger,
            IConfiguration configuration,
            ITweetRepository tweetRepository)
        {
            Logger = logger;
            Configuration = configuration;
            TweetRepository = tweetRepository;

            InitializeTweetMapper();
        }

        private void InitializeTweetMapper()
        {
            var mapperConfig = new MapperConfiguration(
                cfg => { cfg.CreateMap<TweetAnalysis, TweetAnalysis>(); });
            TweetMapper = new Mapper(mapperConfig);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Logger.LogDebug($"Service Event Fired {DateTime.Now:h:mm:ss tt zz}");
                AnalyzeTweets(stoppingToken);
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }

        public void AnalyzeTweets(CancellationToken stoppingToken)
        {
            // TweetRepository.Tweets.All(tweet => tweet.)
            using var disposable = TweetRepository.Tweets.Subscribe(tweet =>
            {
                var tweetToAnalyze = TweetMapper.Map<TweetAnalysis>(Analysis);
                // TweetRepository.Remove();
            });
        }
    }
}