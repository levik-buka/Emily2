using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace emily2.Options
{
    internal static class OptionsExtensions
    {
        internal static ApplicationOptions? LoadUserSecrets(this ApplicationOptions? appSettings, SecretOptions? secretOptions)
        {
            if (string.IsNullOrEmpty(appSettings?.User?.Email))
            {
                // reset secretOptions if no user
                secretOptions?.Reset();
                return appSettings;
            }

            if (appSettings.SecretContainer == SecretContainer.UserContainer)
            {
#pragma warning disable CA1416
                // https://learn.microsoft.com/en-us/dotnet/standard/security/how-to-store-asymmetric-keys-in-a-key-container
                // supported only on Windows (pragma)

                // Create the CspParameters object and set the key container
                // name used to store the RSA key pair.
                var parameters = new CspParameters
                {
                    KeyContainerName = appSettings.User.Email
                };

                // Create a new instance of RSACryptoServiceProvider that accesses
                // the key container MyKeyContainerName.
                appSettings.User.RSA = new RSACryptoServiceProvider(parameters);
#pragma warning restore CA1416
            }
            else // SecretContainer.SettingsFile (secretOptions)
            {
                if (string.IsNullOrEmpty(secretOptions?.PrivateKey))
                {
                    // RSA key missing, re-generate
                    appSettings.User.RSA = RSA.Create();
                }
                else
                {
                    // importing existing RSA key from private PEM key
                    appSettings.User.RSA = new RSACryptoServiceProvider();
                    appSettings.User.RSA.ImportFromPem(secretOptions.PrivateKey);
                }
            }

            return appSettings;
        }

        internal static ApplicationOptions SaveUserSecrets(this ApplicationOptions appSettings, SecretOptions? secretOptions, ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(appSettings);

            if (secretOptions == null) return appSettings;

            // reseting private key in any case.
            // correct key will be set if user defined and key should be saved in secrets.json file
            secretOptions.Reset();

            // if user set
            if (!string.IsNullOrEmpty(appSettings.User?.Email) && appSettings?.SecretContainer == SecretContainer.SettingsFile)
            {
                secretOptions.PrivateKey = appSettings.User.RSA!.ExportRSAPrivateKeyPem();
            }

            secretOptions.SaveSecretOptions(logger);

            return appSettings!;
        }
    }
}
