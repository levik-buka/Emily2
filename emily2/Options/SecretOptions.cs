using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text.Json;
using emily2.Logger;

namespace emily2.Options
{
    internal class SecretOptions(UserOptions userOptions)
    {
        private ILogger _logger = Logger.LoggerExtensions.CreateClassLogger();

        public UserOptions User { get; set; } = userOptions;


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
            _logger.LogTraceMethod(secretsJsonPath);

            // create private roaming folder if missing
            Directory.CreateDirectory(Path.GetDirectoryName(secretsJsonPath));

            // save secrets.json to private roaming folder
            File.WriteAllText(secretsJsonPath, secretsJson);
        }

        internal static string GenerateSecretOptions(SecretOptions secretOptions)
        {
            // serialize SecretOptions
            var updatedSecretsJson = JsonSerializer.Serialize(secretOptions, new JsonSerializerOptions { WriteIndented = true });
            return updatedSecretsJson;
        }
    }
}
