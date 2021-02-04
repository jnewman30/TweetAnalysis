using System;
using Core.Data;
using Core.Data.Model;
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

        [TestMethod]
        public void Total_Count_Is_Updated()
        {
            Assert.Fail("Not Implemented");
        }
    }
}