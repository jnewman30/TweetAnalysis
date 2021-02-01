using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Core.Processing.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tweetinvi;
using Tweetinvi.Core.Models;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters.V2;

namespace Core.Processing
{
    public class TweetProcessingService : BackgroundService, ITweetProcessingService
    {
        private ILogger<TweetProcessingService> Logger { get; }
        private IConfiguration Configuration { get; }

        private string ApiKey => Configuration.GetValue<string>(
            "TwitterApi:ApiKey", null);

        private string ApiSecret => Configuration.GetValue<string>(
            "TwitterApi:ApiSecret", null);

        private string BearerToken => Configuration.GetValue<string>(
            "TwitterApi:BearerToken", null);

        public Subject<TweetV2> Tweets { get; }

        private List<IDisposable> _subs = new List<IDisposable>();

        public TweetProcessingService(
            ILogger<TweetProcessingService> logger,
            IConfiguration configuration)
        {
            Logger = logger;
            Configuration = configuration;
            Tweets = new Subject<TweetV2>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Logger.LogDebug($"Service Event Fired {DateTime.Now:h:mm:ss tt zz}");
                ProcessTweetStream(stoppingToken);
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }

        public async void ProcessTweetStream(CancellationToken stoppingToken)
        {
            Logger.LogDebug($"ApiKey: {ApiKey}");
            Logger.LogDebug($"ApiSecret: {ApiSecret}");
            Logger.LogDebug($"BearerToken: {BearerToken}");

            if (string.IsNullOrWhiteSpace(ApiKey) ||
                string.IsNullOrWhiteSpace(ApiSecret) ||
                string.IsNullOrWhiteSpace(BearerToken))
            {
                Logger.LogCritical("One or all of the following are " +
                                   "missing configuration: ApiKey, BearerToken and ApiUrl.");
                return;
            }

            try
            {
                var client = new TwitterClient(ApiKey, ApiSecret, BearerToken);
                client.Config.TweetMode = TweetMode.Compat;

                _subs.Add(
                    Tweets
                        .LongCount().Subscribe(count => { Logger.LogDebug($"{count} tweets received"); }));

                var sampleStream = client.StreamsV2.CreateSampleStream();
                sampleStream.TweetReceived += (sender, args) =>
                {
                    var tweet = args.Tweet;
                    Logger.LogDebug(args.Json);
                    Tweets.OnNext(tweet);
                };
                await sampleStream.StartAsync();
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error downloading tweet stream.");
            }
        }

        public override void Dispose()
        {
            try
            {
                foreach (var sub in _subs)
                {
                    sub.Dispose();
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error disposing of Rx Subscriptions");
            }
            
            base.Dispose();
        }
    }
}