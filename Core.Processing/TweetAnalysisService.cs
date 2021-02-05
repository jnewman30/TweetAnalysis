using System;
using System.Collections.Generic;
using System.Linq;
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
        private ITweetAnalysisStrategy TweetAnalysisStrategy { get; }
        private Mapper TweetMapper { get; set; }

        public List<TweetAnalysis> Analysis { get; } = new();

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
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }

        public void AnalyzeTweets(CancellationToken stoppingToken)
        {
            var analysis = TweetAnalysisStrategy.Analyze(TweetRepository.Tweets);
            Analysis.Add(analysis);
        }
    }
}