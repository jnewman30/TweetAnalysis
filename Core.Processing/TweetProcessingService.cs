using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Data.Interfaces;
using Core.Processing.Interfaces;
using Core.Processing.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tweetinvi;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters.V2;

namespace Core.Processing
{
    public class TweetProcessingService : BackgroundService, ITweetProcessingService
    {
        private ILogger<TweetProcessingService> Logger { get; }
        private IConfiguration Configuration { get; }
        private ITweetRepository TweetRepository { get; }
        private IEmojiParser EmojiParser { get; }

        private string ApiKey => Configuration.GetValue<string>(
            "TwitterApi:ApiKey", null);

        private string ApiSecret => Configuration.GetValue<string>(
            "TwitterApi:ApiSecret", null);

        private string BearerToken => Configuration.GetValue<string>(
            "TwitterApi:BearerToken", null);

        private Mapper TweetMapper { get; set; }

        public TweetProcessingService(
            ILogger<TweetProcessingService> logger,
            IConfiguration configuration,
            ITweetRepository tweetRepository,
            IEmojiParser emojiParser)
        {
            Logger = logger;
            Configuration = configuration;
            TweetRepository = tweetRepository;
            EmojiParser = emojiParser;
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
                var errorMessage = "One or all of the following are missing configuration: " +
                                   "ApiKey, BearerToken and ApiUrl.";
                Logger.LogCritical(errorMessage);
                throw new Exception(errorMessage);
            }

            try
            {
                // Use this if passed through... <= 0 is no limit (Great for testing)
                var maxTweetCount = Configuration.GetValue(
                    "TwitterApi:MaxTweetCount", -1);

                InitializeTweetMapper();
                EmojiParser.Initialize();
                
                var client = new TwitterClient(ApiKey, ApiSecret, BearerToken);
                client.Config.TweetMode = TweetMode.Extended;
                if (maxTweetCount > 0)
                {
                    client.Config.Limits.MESSAGES_GET_MAX_PAGE_SIZE = 10;
                }

                var sampleStream = client.StreamsV2.CreateSampleStream();
                sampleStream.TweetReceived += (sender, args) =>
                {
                    var tweet = TweetMapper.Map<TweetV2, Data.Model.Tweet>(args.Tweet);
                    tweet.Json = args.Json;
                    tweet.Emojis = EmojiParser.Parse(tweet.Text);
                    TweetRepository.Add(tweet);
                };

                await sampleStream.StartAsync();
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error downloading tweet stream.");
                throw;
            }
        }

        private void InitializeTweetMapper()
        {
            var configurationProvider = TweetMappingProfile.Configure();
            TweetMapper = new Mapper(configurationProvider);
        }
    }
}