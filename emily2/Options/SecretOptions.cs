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

        /// <summary>
        /// Method has side effects
        /// </summary>
        public void SaveSecretOptions()
        {
            var updatedSecretsJson = GenerateSecretOptions(this);

            // get userSecretsId for assembly's meta data
            var userSecretsId = Assembly.GetExecutingAssembly().GetCustomAttribute<UserSecretsIdAttribute>().UserSecretsId;

            // define path to secrets.json in private roaming folder based on userSecretsId (%APPDATA%\Microsoft\UserSecrets)
            var secretsJsonPath = PathHelper.GetSecretsPathFromSecretsId(userSecretsId);

            SaveSecretOptions(secretsJsonPath, updatedSecretsJson);
        }

        internal void SaveSecretOptions(string secretsJsonPath, string secretsJson)
        {
            // create private roaming folder if missing
            Directory.CreateDirectory(Path.GetDirectoryName(secretsJsonPath));

            // save secrets.json to private roaming folder
            File.WriteAllText(secretsJsonPath, secretsJson);
        }

        internal string GenerateSecretOptions(SecretOptions secretOptions)
        {
            // serialize SecretOptions
            var updatedSecretsJson = JsonSerializer.Serialize(secretOptions, new JsonSerializerOptions { WriteIndented = true });
            return updatedSecretsJson;
        }
    }
}
