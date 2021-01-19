using Newtonsoft.Json;

namespace ShortenerApi
{
    public class ShortenerDto
    {
        [JsonProperty("sourcelink")]
        public string SourceLink { get; set; }

        [JsonProperty("shortlink")]
        public string ShortLink { get; set; }

        [JsonProperty("counter")]
        public int Counter { get; set; }
    }
}
