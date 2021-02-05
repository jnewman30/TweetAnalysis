using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core.Data.Model;
using Tweetinvi.Models.V2;

namespace Core.Processing.Mapping
{
    public class DomainResolver : IValueResolver<TweetV2, Tweet, IEnumerable<string>>
    {
        public IEnumerable<string> Resolve(TweetV2 source, Tweet destination, 
            IEnumerable<string> destMember, ResolutionContext context)
        {
            if (source.Entities?.Urls == null)
            {
                return Array.Empty<string>();
            }
            return source.Entities.Urls
                .Select(entity =>
                {
                    var urlToUse = entity?.ExpandedUrl ??
                                   entity?.Url ??
                                   entity?.DisplayUrl;
                    return urlToUse == null 
                        ? string.Empty 
                        : new Uri(urlToUse, UriKind.Absolute).DnsSafeHost;
                });
        }
    }
}