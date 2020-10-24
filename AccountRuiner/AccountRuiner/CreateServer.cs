using Newtonsoft.Json;

namespace AccountRuiner
{
    public class CreateServer
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("icon")]
        public string icon { get; set; }
    }
}
