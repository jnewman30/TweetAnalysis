using System.Threading;

namespace Core.Processing.Interfaces
{
    public interface ITweetAnalysisService
    {
        void AnalyzeTweets(CancellationToken stoppingToken);
    }
}