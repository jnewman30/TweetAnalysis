using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Core.Data.Interfaces;
using Core.Data.Model;

namespace Core.Data
{
    public class TweetRepository : ITweetRepository
    {
        private readonly List<Tweet> _tweetData = new();

        public IEnumerable<Tweet> Tweets => _tweetData;

        public void Add(Tweet item)
        {
            _tweetData.Add(item);
        }

        public bool Remove(Tweet item)
        {
            return _tweetData.Remove(item);
        }
    }
}