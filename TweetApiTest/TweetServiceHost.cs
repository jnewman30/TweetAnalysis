using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Configuration.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TweetApiTest.Interfaces;

namespace TweetApiTest
{
    internal class TweetServiceHost : IHostedService
    {
        private ILogger<TweetServiceHost> Logger { get; }
        private IHostApplicationLifetime AppLifetime { get; }
        private IServiceProvider Services { get; }
        private ISystemConfiguration Configuration { get; }

        private Timer _timer;

        private int StreamIntervalSeconds => Configuration.GetConfigurationValue<int>(
            "TwitterApi", "StreamIntervalSeconds", 30);

        public TweetServiceHost(
            ILogger<TweetServiceHost> logger,
            IHostApplicationLifetime appLifetime,
            IServiceProvider services,
            ISystemConfiguration configuration)
        {
            Logger = logger;
            AppLifetime = appLifetime;
            Services = services;
            Configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("TweetWorker Start Called");
            AppLifetime.ApplicationStarted.Register(OnStarted);
            AppLifetime.ApplicationStopping.Register(OnStopping);
            AppLifetime.ApplicationStopped.Register(OnStopped);

            // _timer = new Timer(DoWork, null, TimeSpan.Zero,
            //     TimeSpan.FromSeconds(StreamIntervalSeconds));
            DoWork();

            return Task.CompletedTask;
        }

        private void DoWork()
        {
            Logger.LogInformation("Timed Background Service is running.");

            using var scope = Services.CreateScope();
            var tweetService = scope
                .ServiceProvider
                .GetRequiredService<ITweetProcessingService>();
            tweetService.ProcessTweetStream();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("TweetWorker Stop Called");
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            Logger.LogInformation("TweetWorker Started");
        }

        private void OnStopping()
        {
            Logger.LogInformation("TweetWorker Stopping");
        }

        private void OnStopped()
        {
            Logger.LogInformation("TweetWorker Stopped");
        }
    }
}