using System;
using System.Collections.Generic;

namespace Core.Data.Model
{
    public class Tweet
    {
        public DateTimeOffset CreatedAt { get; set; }
        public string Json { get; set; }
        public string Text { get; set; }
        public IEnumerable<string> HashTags { get; set; }
        public bool ContainsUrls { get; set; }
        public IEnumerable<string> Urls { get; set; }
        public IEnumerable<string> Photos { get; set; }
        public IEnumerable<string> Domains { get; set; }        
        public IEnumerable<Emoji> Emojis { get; set; }
    }
}