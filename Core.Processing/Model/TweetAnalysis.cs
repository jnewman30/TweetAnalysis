using System;
using System.Collections.Generic;

namespace Core.Processing.Model
{
    public class TweetAnalysis
    {
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public long TotalCount { get; internal set; }

        public double AveragePerHour { get; internal set; }
        
        public double AveragePerMinute { get; internal set; }
        
        public double AveragePerSecond { get; internal set; }
        
        public decimal PercentContainsEmojis { get; internal set; }

        public decimal PercentContainsUrl { get; internal set; }
        
        public decimal PercentContainsPhotoUrl { get; internal set; }
        
        public IEnumerable<string> TopHashTags { get; internal set; }
        
        public IEnumerable<string> TopDomains { get; internal set; }
    }
}