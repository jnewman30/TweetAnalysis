using System.Collections.Generic;

namespace Core.Processing.Model
{
    public record TweetAnalysis
    {
        public long TotalCount { get; init; }
        
        public long AveragePerHour { get; init; }
        
        public long AveragePerMinute { get; init; }
        
        public long AveragePerSecond { get; init; }
        
        public int PercentContainsEmojis { get; init; }

        public int PercentContainsUrl { get; init; }
        
        public int PercentContainsPhotoUrl { get; init; }
        
        public List<string> TopHashTags { get; init; }
        
        public List<string> TopDomains { get; init; }
    }
}