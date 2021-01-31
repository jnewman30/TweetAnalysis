using System;
using System.Linq;
using Core.Configuration.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Configuration
{
    public class SystemConfiguration : IConfigurationSource
    {
        private const string EnvironmentSettingsJson = "environmentSettings.json";

        private ILogger<ISystemConfiguration> Logger { get; }
        private IPathFinder PathFinder { get; }

        public SystemConfiguration(
            ILogger<ISystemConfiguration> logger,
            IPathFinder pathFinder)
        {
            Logger = logger;
            PathFinder = pathFinder;
        }

        // public T GetConfigurationValue<T>(string sectionKey, string valueKey, T defaultValue)
        // {
        //     var section = ConfigurationRoot.GetSection(sectionKey);
        //     try
        //     {
        //         return section.GetValue<T>(valueKey);
        //     }
        //     catch (Exception error)
        //     {
        //         Logger.LogError(error, 
        //             "Error reading system configuration value " +
        //             $"\"{sectionKey}:{valueKey}\". " +
        //             $"Falling back to default value {defaultValue}");
        //     }
        //
        //     return defaultValue;
        // }
        
        private string GetRootConfigPath()
        {
            return PathFinder
                .FindFilePathInAncestors(
                    EnvironmentSettingsJson);
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            var rootConfigPath = GetRootConfigPath();
            return builder
                .AddJsonFile(rootConfigPath)
                .Build()
                .Providers
                .FirstOrDefault();
        }
    }
}