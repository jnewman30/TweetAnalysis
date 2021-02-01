using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Data.Interfaces;
using Core.Processing.Interfaces;
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

        public long TotalCount { get; private set; }

        public TweetAnalysisService(
            ILogger<TweetAnalysisService> logger,
            IConfiguration configuration,
            ITweetRepository tweetRepository)
        {
            Logger = logger;
            Configuration = configuration;
            TweetRepository = tweetRepository;
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
            using var disposable = TweetRepository.Tweets.Subscribe(tweet =>
            {
                TotalCount++;
                TweetRepository.Remove();
            });
        }
    }
}