using emily2.Logger;
using emily2.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NReco.Logging.File;
using System.Security.Cryptography;

IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("logsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile(ApplicationOptions.APPLICATION_OPTIONS_FILE, optional: true, reloadOnChange: false)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .Build();

emily2.Logger.LoggerExtensions.LoggerFactory = LoggerFactory.Create(builder =>
{
    var loggingOptions = config.GetRequiredSection("Logging");
    builder
        .AddConfiguration(loggingOptions)
        .AddFile(loggingOptions); // https://github.com/nreco/logging/tree/master
});

ILogger logger = emily2.Logger.LoggerExtensions.LoggerFactory.CreateLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
using var mainScope = logger.BeginMethodScope();

{
    mainScope.LogTrace("Reading user options");
    SecretOptions userSecret = config.Get<SecretOptions>();
    ApplicationOptions appSettings = config
        .Get<ApplicationOptions>()?
        .LoadUserSecrets(userSecret);


    if (string.IsNullOrEmpty(appSettings.User?.Email))
    {
        appSettings.User = new UserOptions();

        Console.Write("Input user's name: ");
        appSettings.User.UserName = Console.ReadLine();

        Console.Write("Input user's e-mail: ");
        appSettings.User.Email = Console.ReadLine();

        // create new RSA key for the user
        appSettings.LoadUserSecrets(userSecret);
    }

    // Write the values to the console.
    Console.WriteLine($"Username: {appSettings.User.UserName} <{appSettings.User.Email}>");
    Console.WriteLine($"{appSettings.User.RSA.ExportRSAPrivateKeyPem()}");
    Console.WriteLine($"{appSettings.User.RSA.ExportRSAPublicKeyPem()}");
    Console.WriteLine($"Secret container: {appSettings.SecretContainer}");

    Console.Write("Save settings (Y/N)? ");
    ConsoleKeyInfo yesNo = Console.ReadKey(false);

    if (char.ToLower(yesNo.KeyChar) == 'y')
    {
        appSettings
            .SaveUserSecrets(userSecret)
            .SaveApplicationOptions();
    }
}
