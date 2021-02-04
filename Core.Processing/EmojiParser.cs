using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using Core.Data.Model;
using Core.Processing.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Core.Processing
{
    public class EmojiParser : IEmojiParser
    {
        private IConfiguration Configuration { get; }
        
        public IDictionary<string, Emoji> Emojis { get; private set; }

        public int Count => Emojis.Count;        

        public EmojiParser(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Initialize()
        {
            var emojiPath = Configuration.GetValue<string>("EmojiData:Url", null);
            if (string.IsNullOrWhiteSpace(emojiPath))
            {
                throw new ApplicationException("Emoji data source url configuration cannot be null");
            }

            Uri.TryCreate(emojiPath, UriKind.Absolute, out var fileUri);

            string jsonData = null;
            if (fileUri == null || !fileUri.IsFile)
            {
                using var webClient = new WebClient();
                jsonData = webClient.DownloadString(emojiPath);
            }
            else
            {
                var fullPath = Path.GetFullPath(emojiPath);
                jsonData = File.ReadAllText(fullPath);
            }
            
            var emojiData = JsonConvert.DeserializeObject<IEnumerable<Emoji>>(jsonData);
            Emojis = emojiData.ToDictionary(item => item.Unified);            
        }

        public IEnumerable<Emoji> Parse(string text)
        {
            return Emojis.Values
                .Where(emoji => text.Contains(emoji.ToHexSearchString()))
                .ToArray();
        }
    }
}