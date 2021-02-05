using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core.Data.Model;
using Tweetinvi.Models.V2;

namespace Core.Processing.Mapping
{
    public class TweetMappingProfile : Profile
    {
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
            IgnoreEmojiMapping(map);
            MapCreatedAt(map);
            MapUrls(map);
            MapText(map);
            MapHashtags(map);
            MapPhotos(map);
            MapDomains(map);
            MapContainsUrls(map);
        }

        private static void MapUrls(IMappingExpression<TweetV2, Tweet> map)
        {
            map.ForMember(dest => dest.Urls, m => m.MapFrom<UrlV2Resolver>());
        }

        private static void MapCreatedAt(IMappingExpression<TweetV2, Tweet> map)
        {
            map.ForMember(dest => dest.CreatedAt, m => m.MapFrom(src => src.CreatedAt));
        }

        private static void IgnoreJsonMapping(IMappingExpression<TweetV2, Tweet> map)
        {
            map.ForMember(dest => dest.Json, m => m.Ignore());
        }

        private void IgnoreEmojiMapping(IMappingExpression<TweetV2, Tweet> map)
        {
            map.ForMember(dest => dest.Emojis, m => m.Ignore());
        }

        private static void MapHashtags(IMappingExpression<TweetV2, Tweet> map)
        {
            map.ForMember(dest => dest.HashTags, m => m.MapFrom<HashtagV2Resolver>());
        }

        private static void MapText(IMappingExpression<TweetV2, Tweet> map)
        {
            map.ForMember(dest => dest.Text, m => m.MapFrom(src => src.Text));
        }

        private static void MapDomains(IMappingExpression<TweetV2, Tweet> map)
        {
            map.ForMember(dest => dest.Domains, m => m.MapFrom<DomainResolver>());
        }
        
        private void MapPhotos(IMappingExpression<TweetV2, Tweet> map)
        {
            map.ForMember(dest => dest.Photos, m => m.MapFrom<PhotoUrlResolver>());
        }
        
        private static void MapContainsUrls(IMappingExpression<TweetV2, Tweet> map)
        {
            map.ForMember(dest => dest.ContainsUrls, m => m.MapFrom<ContainsUrlsResolver>());
        }        
    }
}