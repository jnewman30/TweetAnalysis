using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Linq;
using Core.Data.Interfaces;

namespace Core.Data
{
    public class TweetRepository : ITweetRepository
    {
        private readonly ConcurrentQueue<Tweet> _tweets = new();

        public IObservable<Tweet> Tweets { get; }

        public TweetRepository()
        {
            Tweets = _tweets.ToObservable();
        }

        public void Add(Tweet item)
        {
            _tweets.Enqueue(item);
        }

        public bool Remove()
        {
            return _tweets.TryDequeue(out var item);
        }
    }
}