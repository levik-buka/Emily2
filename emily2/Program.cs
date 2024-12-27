using emily2;
using emily2.Family;
using emily2.Logger;
using emily2.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NReco.Logging.File;

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

try
{
    mainScope.LogTrace("Reading user options");
    SecretOptions userSecret = config.Get<SecretOptions>();
    ApplicationOptions appSettings = config
        .Get<ApplicationOptions>()?
        .LoadUserSecrets(userSecret);

    EmilyTasks.CheckOrCreateUser(appSettings, userSecret);
    Family family = EmilyTasks.CheckOrCreateProject(appSettings);

    // saving settings before opening the project
    // so settings will be saved even if application falls
    appSettings
        .SaveUserSecrets(userSecret)
        .SaveApplicationOptions();

    if (family != null)
    {

    }
}
catch (Exception e)
{
    logger.LogCritical(e, "Exiting application because of ");
}

return 0;
