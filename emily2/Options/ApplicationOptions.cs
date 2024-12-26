using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace emily2.Options
{
    internal class ApplicationOptions(UserOptions userOptions)
    {
        public static readonly string APPLICATION_OPTIONS_FILE = "appsettings.json";

        public UserOptions User { get; set; } = userOptions;

        /// <summary>
        /// Method has side effects
        /// </summary>
        public void SaveApplicationOptions()
        {
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
