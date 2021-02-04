using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public void Parse_Emoji_Test()
        {
            var emojiData = File.ReadAllText("./emoji.json");
            var timelineData = File.ReadAllText("./sample_timeline.json");
            
            var emojiJson = JArray.Parse(emojiData);
            var timelineJson = JObject.Parse(timelineData)["data"].Children();
        }
        
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
            var sampleTweet = JObject.Parse(sampleTweetJson);
            var sampleTweetText = sampleTweet["data"]?.Children().First().Value<string>("text");
            var actualResult = emojiParser.Parse(sampleTweetText);
            Assert.IsNotNull(actualResult);
        }
    }
}