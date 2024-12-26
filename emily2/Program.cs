using emily2.Logger;
using emily2.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NReco.Logging.File;

IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .Build();

emily2.Logger.LoggerExtensions.LoggerFactory = LoggerFactory.Create(builder =>
{
    var loggingOptions = config.GetRequiredSection("Logging");
    builder.AddConfiguration(loggingOptions);
    builder.AddFile(loggingOptions); // https://github.com/nreco/logging/tree/master
});

ILogger logger = emily2.Logger.LoggerExtensions.LoggerFactory.CreateLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
using var mainScope = logger.BeginMethodScope();

{
    mainScope.LogTrace("Reading user options");
    UserOptions userOptions = config.GetRequiredSection("User").Get<UserOptions>();

    // Write the values to the console.
    Console.WriteLine($"KeyOne = {userOptions.UserName}");
    Console.WriteLine($"KeyTwo = {userOptions.Email}");
    Console.WriteLine($"PrivateKey = {userOptions.PrivateKey}");

    userOptions.PrivateKey = Guid.NewGuid().ToString();
    Console.WriteLine($"New PrivateKey = {userOptions.PrivateKey}");

    var secretOptions = new SecretOptions(userOptions);
    secretOptions.SaveSecretOptions();
}
