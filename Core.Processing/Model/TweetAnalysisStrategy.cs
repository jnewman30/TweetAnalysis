using System.Collections.Generic;
using Core.Data.Model;
using Core.Processing.Interfaces;

namespace Core.Processing.Model
{
    public class TweetAnalysisStrategy : ITweetAnalysisStrategy
    {
        public TweetAnalysis Analyze(IEnumerable<Tweet> tweets)
        {
            var data = new TweetAnalysis();

            return data;
        }
    }
}