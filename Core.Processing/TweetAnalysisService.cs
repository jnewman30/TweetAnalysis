using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
    public class TweetAnalysisService : BackgroundService
    {
        private ILogger<TweetAnalysisService> Logger { get; }
        private IConfiguration Configuration { get; }
        private ITweetRepository TweetRepository { get; }
        private ITweetAnalysisStrategy TweetAnalysisStrategy { get; }
        private Mapper TweetMapper { get; set; }

        private long _currentTweetCount = 0;

        public TweetAnalysis Analysis { get; private set; }

        public TweetAnalysisService(
            ILogger<TweetAnalysisService> logger,
            IConfiguration configuration,
            ITweetRepository tweetRepository,
            ITweetAnalysisStrategy tweetAnalysisStrategy)
        {
            Logger = logger;
            Configuration = configuration;
            TweetRepository = tweetRepository;
            TweetAnalysisStrategy = tweetAnalysisStrategy;

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
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        public void AnalyzeTweets(CancellationToken stoppingToken)
        {
            try
            {
                if (!TweetRepository.Tweets.Any())
                {
                    return;
                }

                var tweetCount = TweetRepository.Tweets.Count();
                if (tweetCount == _currentTweetCount)
                {
                    return;
                }

                Analysis = TweetAnalysisStrategy.Analyze(TweetRepository.Tweets);

                _currentTweetCount = tweetCount;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error analyzing tweets");
            }
        }
    }
}