using Microsoft.Extensions.Configuration.UserSecrets;
using System.Reflection;
using System.Text.Json;

namespace emily2.Options
{
    internal class SecretOptions
    {
        public UserOptions User { get; set; }

        public SecretOptions(UserOptions userOptions) 
        { 
            User = userOptions;
        }

        public void SaveSecretOptions()
        {
            var secretsId = Assembly.GetExecutingAssembly().GetCustomAttribute<UserSecretsIdAttribute>().UserSecretsId;
            var secretsPath = PathHelper.GetSecretsPathFromSecretsId(secretsId);
            Directory.CreateDirectory(Path.GetDirectoryName(secretsPath));

            var updatedSecretsJson = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(secretsPath, updatedSecretsJson);
        }
    }
}
