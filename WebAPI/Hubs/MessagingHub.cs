using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace WebAPI.Hubs
{
    public class MessagingHub : Hub
    {
        private const string WebCallbackFunction = "ReceiveTweetAnalysis";

        private ILogger<MessagingHub> Logger { get; }

        public MessagingHub(ILogger<MessagingHub> logger)
        {
            Logger = logger;
        }

        public async Task SendTweetAnalysisAsync(string message)
        {
            try
            {
                var responseMessage = $"Received {message}";
                Logger.LogDebug(responseMessage);

                // Passthrough for testing
                await Clients.All.SendAsync(
                    WebCallbackFunction,
                    responseMessage);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error fetching tweet stream analysis.");
            }
        }
    }
}