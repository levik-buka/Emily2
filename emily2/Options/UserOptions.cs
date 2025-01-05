using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace emily2.Options
{
    internal class UserOptions
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        [JsonIgnore]
        public RSA? RSA { get; set; }
    }
}
