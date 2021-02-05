using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core.Data.Model;
using Tweetinvi.Models.V2;

namespace Core.Processing.Mapping
{
    public class PhotoUrlResolver : IValueResolver<TweetV2, Tweet, IEnumerable<string>>
    {
        public static IEnumerable<string> PhotoUrlMatches { get; set; } = new[]
        {
            "pic.twitter.com",
            "instagram"
        };

        public IEnumerable<string> Resolve(TweetV2 source, Tweet destination, 
            IEnumerable<string> destMember, ResolutionContext context)
        {
            if (source.Entities?.Urls == null)
            {
                return Array.Empty<string>();
            }
            
            return source.Entities.Urls
                .Where(entity => PhotoUrlMatches
                    .Any(match =>
                    {
                        var matchString = entity?.ExpandedUrl ?? 
                                          entity?.Url ??
                                          entity?.DisplayUrl;
                        return matchString != null && 
                               matchString.Contains(match, 
                                   StringComparison.CurrentCultureIgnoreCase);
                    }))
                .Select(src => src.ExpandedUrl);
        }
    }
}