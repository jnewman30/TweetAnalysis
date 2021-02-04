using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Core.Data.Model
{
    public class Emoji
    {
        public string Name { get; set; }
        
        public string Unified { get; set; }

        [JsonProperty("non_qualified")]
        public string NonQualified { get; set; }
        
        public string Docomo { get; set; }
        
        public string Au { get; set; }
        
        public string SoftBank { get; set; }
        
        public string Google { get; set; }
        
        public string Image { get; set; }
        
        [JsonProperty("sheet_x")]
        public int SheetX { get; set; }
        
        [JsonProperty("sheet_y")]
        public int SheetY { get; set; }
        
        [JsonProperty("short_name")]
        public string ShortName { get; set; }
        
        [JsonProperty("short_names")]
        public IEnumerable<string> ShortNames { get; set; }
        
        public string Text { get; set; }
        
        public IEnumerable<string> Texts { get; set; }
        
        public string Category { get; set; }
        
        [JsonProperty("sort_order")]
        public int SortOrder { get; set; }
        
        [JsonProperty("added_in")]
        public string AddedIn { get; set; }
        
        [JsonProperty("has_img_apple")]
        public bool HasImageApple { get; set; }
        
        [JsonProperty("has_img_google")]
        public bool HasImageGoogle { get; set; }
        
        [JsonProperty("has_img_twitter")]
        public bool HasImageTwitter { get; set; }
        
        [JsonProperty("has_img_facebook")]
        public bool HasImageFacebook { get; set; }

        public string ToHexSearchString()
        {
            return string.Join(string.Empty, 
                Unified.Split('-')
                    .Select(hex => char.ConvertFromUtf32(
                        Convert.ToInt32(hex, 16))));            
        }
    }
}