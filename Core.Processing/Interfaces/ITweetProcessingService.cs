using System.Threading;

namespace Core.Processing.Interfaces
{
    public interface ITweetProcessingService
    {
        void ProcessTweetStream(CancellationToken stoppingToken);
    }
}