using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core.Data.Model;
using Tweetinvi.Models.V2;

namespace Core.Processing.Mapping
{
    public class HashtagV2Resolver : IValueResolver<TweetV2, Tweet, IEnumerable<string>>
    {
        public IEnumerable<string> Resolve(TweetV2 source, Tweet destination, 
            IEnumerable<string> destMember, ResolutionContext context)
        {
            return source.Entities?.Hashtags?
                .Select(src => src.Tag).ToArray() ?? Array.Empty<string>();
        }
    }
}