using System.Collections.Generic;
using System.Linq;
using Core.Data.Model;
using Core.Processing.Interfaces;

namespace Core.Processing.Model
{
    public class TweetAnalysisStrategy : ITweetAnalysisStrategy
    {
        public TweetAnalysis Analyze(IEnumerable<Tweet> tweets)
        {
            // Shortcut possible deferred execution...
            var dataToAnalyze = tweets.ToArray();

            var dataToReturn = new TweetAnalysis { TotalCount = dataToAnalyze.Count() };

            ComputeTopDomains(dataToAnalyze, dataToReturn);

            ComputeTopHashtags(dataToAnalyze, dataToReturn);

            ComputeAveragePerHour(dataToAnalyze, dataToReturn);

            ComputeAveragePerMinute(dataToAnalyze, dataToReturn);

            ComputeAveragePerSecond(dataToAnalyze, dataToReturn);

            ComputePercentContainsEmojis(dataToAnalyze, dataToReturn);

            ComputePercentContainsUrl(dataToAnalyze, dataToReturn);

            ComputePercentContainsPhotos(dataToAnalyze, dataToReturn);

            return dataToReturn;
        }

        private void ComputePercentContainsUrl(
            IReadOnlyCollection<Tweet> dataToAnalyze, TweetAnalysis dataToReturn)
        {
            var urlCount = dataToAnalyze
                .Count(tweet => tweet.ContainsUrls);

            dataToReturn.PercentContainsUrl =
                decimal.Round(urlCount / (decimal) dataToAnalyze.Count * 100m, 2);
        }

        private void ComputePercentContainsEmojis(
            IReadOnlyCollection<Tweet> dataToAnalyze, TweetAnalysis dataToReturn)
        {
            var emojiCount = dataToAnalyze
                .Count(tweet => tweet.Emojis.Any());

            dataToReturn.PercentContainsEmojis =
                decimal.Round(emojiCount / (decimal) dataToAnalyze.Count * 100m, 2);
        }

        private void ComputePercentContainsPhotos(
            IReadOnlyCollection<Tweet> dataToAnalyze, TweetAnalysis dataToReturn)
        {
            var photoCount = dataToAnalyze
                .Count(tweet => tweet.Photos.Any());

            dataToReturn.PercentContainsPhotoUrl =
                decimal.Round(photoCount / (decimal) dataToAnalyze.Count * 100m, 2);
        }

        private void ComputeAveragePerHour(
            IReadOnlyCollection<Tweet> dataToAnalyze, TweetAnalysis dataToReturn)
        {
            var groupedByHour = dataToAnalyze
                .GroupBy(d => new
                {
                    d.CreatedAt.Year,
                    d.CreatedAt.Month,
                    d.CreatedAt.Day,
                    d.CreatedAt.Hour
                });

            var countByHour = groupedByHour.Select(grp => new
            {
                grp.Key.Year,
                grp.Key.Month,
                grp.Key.Day,
                grp.Key.Hour,
                Count = grp.Count()
            }).ToArray();

            dataToReturn.AveragePerHour =
                countByHour.Average(grp => grp.Count);
        }

        private void ComputeAveragePerMinute(
            IReadOnlyCollection<Tweet> dataToAnalyze, TweetAnalysis dataToReturn)
        {
            var tweetsPerMinute = dataToAnalyze
                .GroupBy(d => new
                {
                    d.CreatedAt.Year,
                    d.CreatedAt.Month,
                    d.CreatedAt.Day,
                    d.CreatedAt.Hour,
                    d.CreatedAt.Minute
                })
                .Select(grp => new
                {
                    grp.Key.Year,
                    grp.Key.Month,
                    grp.Key.Day,
                    grp.Key.Hour,
                    grp.Key.Minute,
                    Count = grp.Count()
                })
                .ToArray();

            dataToReturn.AveragePerMinute =
                tweetsPerMinute.Average(d => d.Count);
        }

        private void ComputeAveragePerSecond(
            IReadOnlyCollection<Tweet> dataToAnalyze, TweetAnalysis dataToReturn)
        {
            var tweetsPerSecond = dataToAnalyze
                .GroupBy(d => new
                {
                    d.CreatedAt.Year,
                    d.CreatedAt.Month,
                    d.CreatedAt.Day,
                    d.CreatedAt.Hour,
                    d.CreatedAt.Minute,
                    d.CreatedAt.Second
                })
                .Select(grp => new
                {
                    grp.Key.Year,
                    grp.Key.Month,
                    grp.Key.Day,
                    grp.Key.Hour,
                    grp.Key.Minute,
                    grp.Key.Second,
                    Count = grp.Count()
                })
                .ToArray();

            dataToReturn.AveragePerSecond =
                tweetsPerSecond.Average(d => d.Count);
        }

        private static void ComputeTopHashtags(
            IReadOnlyCollection<Tweet> tweets, TweetAnalysis data)
        {
            data.TopHashTags = tweets
                .SelectMany(d => d.HashTags)
                .GroupBy(d => d)
                .OrderByDescending(grp => grp.Count())
                .Take(10)
                .Select(grp => grp.Key)
                .ToArray();
        }

        private static void ComputeTopDomains(
            IReadOnlyCollection<Tweet> tweets, TweetAnalysis data)
        {
            data.TopDomains = tweets
                .SelectMany(d => d.Domains)
                .GroupBy(d => d)
                .OrderByDescending(grp => grp.Count())
                .Take(10)
                .Select(grp => grp.Key)
                .ToArray();
        }
    }
}