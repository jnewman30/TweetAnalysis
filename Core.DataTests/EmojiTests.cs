using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Data.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.DataTests
{
    [TestClass]
    public class EmojiTests
    {
        [TestMethod]
        public void Can_Deserialize()
        {
            var data = File.ReadAllText("./emoji.json");
            var actual = JsonConvert.DeserializeObject<List<Emoji>>(data);
            Assert.IsNotNull(actual);
            Assert.AreEqual(1810, actual.Count);
            
            var expected = JArray.Parse(data);
            foreach (var expectedItem in expected)
            {
                var actualItem = actual.FirstOrDefault(o =>
                    o.Unified == (expectedItem["unified"] ?? string.Empty).Value<string>());
                Assert.IsNotNull(actualItem);
                Assert.AreEqual(actualItem.Au, (expectedItem["au"] ?? string.Empty).Value<string>());
                Assert.AreEqual(actualItem.Category, (expectedItem["category"] ?? string.Empty).Value<string>());
                Assert.AreEqual(actualItem.Docomo, (expectedItem["docomo"] ?? string.Empty).Value<string>());
                Assert.AreEqual(actualItem.Google, (expectedItem["google"] ?? string.Empty).Value<string>());
                Assert.AreEqual(actualItem.Image, (expectedItem["image"] ?? string.Empty).Value<string>());
                Assert.AreEqual(actualItem.Name, (expectedItem["name"] ?? string.Empty).Value<string>());
            }
        }
    }
}