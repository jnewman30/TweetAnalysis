using System.Collections.Generic;
using Core.Data.Model;
using Core.Processing.Model;

namespace Core.Processing.Interfaces
{
    public interface ITweetAnalysisStrategy
    {
        TweetAnalysis Analyze(IEnumerable<Tweet> tweets);
    }
}