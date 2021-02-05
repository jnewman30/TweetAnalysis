using System.Linq;
using AutoMapper;
using Core.Data.Model;
using Tweetinvi.Models.V2;

namespace Core.Processing.Mapping
{
    public class ContainsUrlsResolver : IValueResolver<TweetV2, Tweet, bool>
    {
        public bool Resolve(TweetV2 source, Tweet destination,
            bool destMember, ResolutionContext context)
        {
            return source.Entities?.Urls != null && source.Entities.Urls.Any();
        }
    }
}