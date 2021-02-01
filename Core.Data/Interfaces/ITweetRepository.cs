﻿using System;

namespace Core.Data.Interfaces
{
    public interface ITweetRepository
    {
        IObservable<Tweet> Tweets { get; }
        void Add(Tweet item);
        bool Remove();
    }
}