using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text.Json;
using emily2.Logger;

namespace emily2.Options
{
    internal class SecretOptions
    {
        public string? PrivateKey { get; set; }

        internal void Reset()
        {
            PrivateKey = null;
        }

        /// <summary>
        /// Method has side effects
        /// </summary>
        public void SaveSecretOptions(ILogger logger)
        {
            var updatedSecretsJson = GenerateSecretOptions(this);

            // get userSecretsId for assembly's meta data
            var userSecretsId = Assembly.GetExecutingAssembly().GetCustomAttribute<UserSecretsIdAttribute>()!.UserSecretsId;

            // define path to secrets.json in private roaming folder based on userSecretsId (%APPDATA%\Microsoft\UserSecrets)
            var secretsJsonPath = PathHelper.GetSecretsPathFromSecretsId(userSecretsId);
                
            SaveSecretOptions(secretsJsonPath, updatedSecretsJson, logger);
        }

        internal static void SaveSecretOptions(string secretsJsonPath, string secretsJson, ILogger logger)
        {
            logger.LogTraceMethod(secretsJsonPath);

            // create private roaming folder if missing
            Directory.CreateDirectory(Path.GetDirectoryName(secretsJsonPath)!);

            // save secrets.json to private roaming folder
            File.WriteAllText(secretsJsonPath, secretsJson);
        }

        internal static string GenerateSecretOptions(SecretOptions secretOptions)
        {
#pragma warning disable CA1869
            // serialize SecretOptions
            var updatedSecretsJson = JsonSerializer.Serialize(secretOptions, new JsonSerializerOptions { WriteIndented = true });
            return updatedSecretsJson;
#pragma warning restore CA1869
        }
    }
}
