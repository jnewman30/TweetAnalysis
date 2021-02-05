using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Configuration;
using Core.Data;
using Core.Data.Interfaces;
using Core.Data.Model;
using Core.Processing;
using Core.Processing.Interfaces;
using Core.Processing.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.ProcessingTests
{
    [TestClass]
    public class TweetProcessingServiceTests
    {
        [TestMethod]
        public void Can_Process_Tweet_Stream()
        {
            var pathFinder = new PathFinder();
            var configPath = pathFinder.FindFilePathInAncestors("environmentSettings.json");
            
            var config = new ConfigurationBuilder()
                .AddJsonFile(configPath)
                .Build();

            var services = new ServiceCollection()
                .AddSingleton<IConfiguration>(p => config)
                .AddLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .AddSingleton<IEmojiParser, EmojiParser>()
                .AddSingleton<ITweetRepository, TweetRepository>()
                .AddSingleton<ITweetAnalysisStrategy, TweetAnalysisStrategy>()
                .AddSingleton<ITweetProcessingService, TweetProcessingService>()
                .BuildServiceProvider();

            var repo = services.GetService<ITweetRepository>();
            Assert.IsNotNull(repo);

            var processingService = services.GetService<ITweetProcessingService>();
            Assert.IsNotNull(processingService);

            var cancellationToken = new CancellationToken();
            processingService.ProcessTweetStream(cancellationToken);

                cancellationToken
                    .WaitHandle
                    .WaitOne(TimeSpan.FromSeconds(10));

                Assert.IsTrue(repo.Tweets.Any());
        }
    }
}