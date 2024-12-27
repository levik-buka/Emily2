using emily2.Logger;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace emily2.Options
{
    [JsonConverter(typeof(JsonStringEnumConverter<SecretContainer>))]
    public enum SecretContainer
    {
        /// <summary>
        /// User-level Key Container
        /// </summary>
        UserContainer,
        /// <summary>
        /// secrets.json
        /// </summary>
        SettingsFile
    }

    internal class ApplicationOptions
    {
        public static readonly string APPLICATION_OPTIONS_FILE = "appsettings.json";

        private ILogger _logger = Logger.LoggerExtensions.CreateClassLogger();

        public UserOptions User { get; set; }

        public SecretContainer SecretContainer { get; set; }

        /// <summary>
        /// Method has side effects
        /// </summary>
        public void SaveApplicationOptions()
        {
            _logger.LogTraceMethod();

            var updatedOptionsJson = GenerateApplicationOptions(this);

            // save appsetting.json to working folder
            File.WriteAllText(APPLICATION_OPTIONS_FILE, updatedOptionsJson);
        }

        internal static string GenerateApplicationOptions(ApplicationOptions options)
        {
            // serialize SecretOptions
            var updatedSecretsJson = JsonSerializer.Serialize(options, new JsonSerializerOptions { WriteIndented = true });
            return updatedSecretsJson;
        }
    }
}
