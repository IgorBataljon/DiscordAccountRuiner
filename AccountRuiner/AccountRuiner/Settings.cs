using Newtonsoft.Json;

namespace AccountRuiner
{
    public class Settings
    {
        [JsonProperty("avatar")]
        public string avatar { get; set; }

        [JsonProperty("discriminator")]
        public readonly string discriminator = null;

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("new_password")]
        public readonly string newPassword = null;

        [JsonProperty("password")]
        public readonly string password = "";

        [JsonProperty("username")]
        public string username { get; set; }
    }
}