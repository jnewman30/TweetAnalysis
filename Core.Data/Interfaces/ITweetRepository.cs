using System.Collections.Generic;
using Core.Data.Model;

namespace Core.Data.Interfaces
{
    public interface ITweetRepository
    {
        IEnumerable<Tweet> Tweets { get; }
        
        void Add(Tweet item);
        
        bool Remove(Tweet item);
    }
}