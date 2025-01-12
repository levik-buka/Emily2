using emily2.Logger;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
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

    public class ApplicationOptions
    {
        public static readonly string APPLICATION_OPTIONS_FILE = "appsettings.json";

        public UserOptions? User { get; set; }

        public SecretContainer SecretContainer { get; set; }

        private string? _projectsBasePath;
        public string? ProjectsBasePath 
        {
            get
            {
                if (string.IsNullOrEmpty(_projectsBasePath))
                {
                    _projectsBasePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Emily";
                }
                return _projectsBasePath;
            }

            set { _projectsBasePath = value; }
        }

        internal string? GetProjectPathForFamily(string? family)
        {
            if (string.IsNullOrEmpty(family))
            {
                return null;
            }

            return ProjectsBasePath + "\\" + family;
        }

        public string? LastProjectName { get; set; }

        /// <summary>
        /// Method has side effects
        /// </summary>
        public void SaveApplicationOptions(ILogger logger)
        {
            logger.LogTraceMethod();

            var updatedOptionsJson = GenerateApplicationOptions(this);

            // save appsetting.json to working folder
            File.WriteAllText(APPLICATION_OPTIONS_FILE, updatedOptionsJson);
        }

        internal static string GenerateApplicationOptions(ApplicationOptions options)
        {
#pragma warning disable CA1869
            // serialize SecretOptions
            var updatedSecretsJson = JsonSerializer.Serialize(options, new JsonSerializerOptions { WriteIndented = true });
            return updatedSecretsJson;
#pragma warning restore CA1869
        }
    }
}
