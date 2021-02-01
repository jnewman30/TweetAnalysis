using System;
using Core.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.DataTests
{
    [TestClass]
    public class TweetRepositoryTests
    {
        [TestMethod]
        public void Can_Add_Tweet_Json()
        {
            var expected = new Tweet
            {
                Json = Guid.NewGuid().ToString()
            };
            
            var repo = new TweetRepository();

            repo.Tweets.Subscribe(actual =>
            {
                Assert.AreEqual(expected.Json, actual.Json);
            });
            
            repo.Add(expected);            
        }
    }
}