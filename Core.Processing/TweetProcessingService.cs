﻿using System;
using System.Net;
using Core.Configuration.Interfaces;
using Core.Processing.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Processing
{
    public class TweetProcessingService : ITweetProcessingService
    {
        private ILogger<TweetProcessingService> Logger { get; }
        private IConfiguration Configuration { get; }

        private string ApiKey => Configuration.GetValue<string>(
            "TwitterApi:ApiKey", null);

        private string BearerToken => Configuration.GetValue<string>(
            "TwitterApi:BearerToken", null);

        private string ApiUrl => Configuration.GetValue<string>(
            "TwitterApi:ApiUrl", null);

        public TweetProcessingService(
            ILogger<TweetProcessingService> logger,
            IConfiguration configuration)
        {
            Logger = logger;
            Configuration = configuration;
        }

        public void ProcessTweetStream()
        {
            Logger.LogDebug($"ApiKey: {ApiKey}");
            Logger.LogDebug($"BearerToken: {BearerToken}");
            Logger.LogDebug($"ApiUrl: {ApiUrl}");

            if (string.IsNullOrWhiteSpace(ApiKey) ||
                string.IsNullOrWhiteSpace(BearerToken) ||
                string.IsNullOrWhiteSpace(ApiUrl))
            {
                Logger.LogCritical("One or all of the following are " +
                                   "missing configuration: ApiKey, BearerToken and ApiUrl.");
                return;
            }

            try
            {
                var time = DateTime.Now;
                var fileNameSuffix =
                    $"{time.Year:0000}{time.Month:00}{time.Day:00}{time.Hour:00}{time.Minute:00}{time.Second:00}";
                using var webClient = new WebClient();
                webClient.Headers.Add("Authorization", $"Bearer {BearerToken}");
                // webClient.DownloadFile(ApiUrl, $"./tweets-{fileNameSuffix}.json");
                
                Logger.LogDebug("File appears to have been written.");
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error downloading tweet stream.");
            }
        }
    }
}