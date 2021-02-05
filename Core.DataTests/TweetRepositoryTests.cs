using System;
using System.Linq;
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
            repo.Add(expected);

            Assert.IsTrue(repo.Tweets.Count() == 1);
            Assert.AreEqual(expected.Json, repo.Tweets.FirstOrDefault()?.Json);
        }
    }
}