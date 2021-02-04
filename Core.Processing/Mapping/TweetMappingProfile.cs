using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core.Data.Model;
using Tweetinvi.Models;
using Tweetinvi.Models.V2;

namespace Core.Processing.Mapping
{
    public class TweetMappingProfile : Profile
    {
        public IEnumerable<string> PhotoUrlMatches { get; set; } = new[]
        {
            "pic.twitter.com",
            "instagram"
        };

        public TweetMappingProfile()
        {
            InitializeMappings();
        }

        public static IConfigurationProvider Configure()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TweetMappingProfile>();
            });
        }

        private void InitializeMappings()
        {
            var map = CreateMap<TweetV2, Tweet>();

            IgnoreJsonMapping(map);

            MapCreatedAt(map);

            MapUrls(map);
            
            MapText(map);

            IgnoreEmojiMapping(map);

            MapHashtags(map);

            MapContainsUrls(map);

            MapPhotos(map);

            MapDomains(map);
        }

        private static void MapUrls(IMappingExpression<TweetV2, Tweet> map)
        {
            map.ForMember(dest => dest.Urls,
                options => options.MapFrom(src => src.Entities.Urls));
        }

        private static void MapCreatedAt(IMappingExpression<TweetV2, Tweet> map)
        {
            map.ForMember(dest => dest.CreatedAt, options => options.MapFrom(src => src.CreatedAt));
        }

        private static void IgnoreJsonMapping(IMappingExpression<TweetV2, Tweet> map)
        {
            map.ForMember(dest => dest.Json, options => options.Ignore());
        }
        
        private void IgnoreEmojiMapping(IMappingExpression<TweetV2,Tweet> map)
        {
            map.ForMember(dest => dest.Emojis, options => options.Ignore());
        }

        private static void MapDomains(IMappingExpression<TweetV2, Tweet> map)
        {
            map.ForMember(dest => dest.Domains, options =>
            {
                options.AllowNull();
                options.MapFrom(src => src.Entities.Urls
                    .Select(entity => new Uri(entity.UnwoundUrl, UriKind.Absolute).DnsSafeHost));
            });
        }

        private void MapPhotos(IMappingExpression<TweetV2, Tweet> map)
        {
            map.ForMember(dest => dest.Photos, options =>
            {
                options.AllowNull();
                options.NullSubstitute(Array.Empty<string>());
                options.MapFrom(src => src.Entities.Urls
                    .Where(entity => PhotoUrlMatches
                        .Any(match => entity.UnwoundUrl
                            .Contains(match, StringComparison.CurrentCultureIgnoreCase))));
            });
        }

        private static void MapContainsUrls(IMappingExpression<TweetV2, Tweet> map)
        {
            map.ForMember(dest => dest.ContainsUrls, options =>
            {
                options.DoNotAllowNull();
                options.NullSubstitute(Array.Empty<string>());
                options.MapFrom(src => src.Entities.Urls.Any());
            });
        }

        private static void MapHashtags(IMappingExpression<TweetV2, Tweet> map)
        {
            map.ForMember(dest => dest.HashTags, options => options
                .MapFrom(src => src.Entities.Hashtags.Select(s => s.Tag)));
        }

        private static void MapText(IMappingExpression<TweetV2, Tweet> map)
        {
            map.ForMember(dest => dest.Text, options => options
                .MapFrom(src => src.Text));
        }
    }
}