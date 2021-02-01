using System.Collections.Generic;

namespace Core.Processing.Model
{
    public record TweetAnalysis
    {
        public long TotalCount { get; }
        
        public long AveragePerHour { get; }
        
        public long AveragePerMinute { get; }
        
        public long AveragePerSecond { get; }
        
        public int PercentContainsEmojis { get; }

        public int PercentContainsUrl { get; }
        
        public int PercentContainsPhotoUrl { get; set; }
        
        public List<string> TopHashTags { get; }
        
        public List<string> TopDomains { get; }
    }
}