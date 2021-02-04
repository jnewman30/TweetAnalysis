using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Data.Model;
using Core.Processing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Core.ProcessingTests
{
    [TestClass]
    public class EmojiParserTests
    {
        [TestMethod]
        public void Can_Load_Emojis_From_File()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>(
                        "EmojiData:Url", "./emoji.json")
                }).Build();

            var emojiParser = new EmojiParser(config);
            emojiParser.Initialize();

            Assert.IsNotNull(emojiParser.Emojis);
            Assert.IsTrue(emojiParser.Count > 0);
        }

        [TestMethod]
        public void Can_Load_Emojis_From_Url()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>(
                        "EmojiData:Url", "https://cdn.jsdelivr.net/npm/emoji-datasource@6.0.0/emoji.json")
                }).Build();

            var emojiParser = new EmojiParser(config);
            emojiParser.Initialize();

            Assert.IsNotNull(emojiParser.Emojis);
            Assert.IsTrue(emojiParser.Count > 0);
        }

        [TestMethod]
        public void Can_Parse_Emojis_From_Text()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>(
                        "EmojiData:Url", "./emoji.json")
                }).Build();

            var emojiParser = new EmojiParser(config);
            emojiParser.Initialize();

            var sampleTweetJson = File.ReadAllText("./sample_timeline.json");
            var sampleTweetObj = JObject.Parse(sampleTweetJson);
            var sampleTweets = sampleTweetObj["data"]?.Children();

            var actual = new List<IEnumerable<Emoji>>();

            foreach (var sampleTweet in sampleTweets)
            {
                var text = sampleTweet.Value<string>("text");
                var actualResult = emojiParser.Parse(text);
                Assert.IsNotNull(actualResult);
                actual.Add(actualResult);
            }

            Assert.IsTrue(actual.ElementAt(0).Count() == 1);
            Assert.IsTrue(actual.ElementAt(1).Count() == 1);
            Assert.IsTrue(actual.ElementAt(2).Count() == 2); // Should be 3 as one appears twice...
            Assert.IsTrue(actual.ElementAt(3).Count() == 3);
        }
    }
}