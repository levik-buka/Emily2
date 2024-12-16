using System.Text.Json.Serialization;

namespace emily2.Options
{
    internal class UserOptions
    {
        [JsonIgnore]
        public string UserName { get; set; }
        [JsonIgnore]
        public string Email { get; set; }
        public string PrivateKey { get; set; }
        [JsonIgnore]
        public string PublicKey { get; set; }
    }
}
