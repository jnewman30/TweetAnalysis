using System;
using Core.Configuration;
using Core.Configuration.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TweetApiTest.Interfaces;

namespace TweetApiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using var host = Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddTransient<IPathFinder, PathFinder>()
                        .AddTransient<ISystemConfiguration, SystemConfiguration>()
                        .AddTransient<ITweetProcessingService, TweetProcessingService>()
                        .AddHostedService<TweetServiceHost>();
                })
                .Build();
            host.Run();
            // host.StartAsync();
            // // CTRL-C to Shutdown...
            // host.WaitForShutdown();
        }
    }
}