﻿using emily2;
using emily2.Family;
using emily2.Logger;
using emily2.Options;
using emily2.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NReco.Logging.File;

IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("logsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile(ApplicationOptions.APPLICATION_OPTIONS_FILE, optional: true, reloadOnChange: false)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .Build();

ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    var loggingOptions = config.GetRequiredSection("Logging");
    builder
        .AddConfiguration(loggingOptions)
        .AddFile(loggingOptions); // https://github.com/nreco/logging/tree/master
});

// create MAIN logger
ILogger logger = loggerFactory.CreateLogger(System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!);
using var mainScope = logger.BeginMethodScope();

try
{
    mainScope.LogTrace("Reading user options");
    SecretOptions userSecret = config.Get<SecretOptions>()!;
    ApplicationOptions? appSettings = config
        .Get<ApplicationOptions>()?
        .LoadUserSecrets(userSecret);

    appSettings = EmilyTasks.CheckOrCreateUser(appSettings, userSecret);
    if (appSettings == null) return 0; // exiting

    Family? family = EmilyTasks.CheckOrCreateProject(appSettings, loggerFactory);

    // saving settings before opening the project
    // so settings will be saved even if application falls
    appSettings
        .SaveUserSecrets(userSecret, logger)
        .SaveApplicationOptions(logger);

    if (family == null) return 0;   // exiting

    IFamilyRepository repository = appSettings.CreateFamilyRepository(loggerFactory);
    repository.LoadFamilyMembers(family);
    repository.AddUserToFamilyMembers(family);

    EmilyTasks.PrintFamilyMembers(family);
}
catch (Exception e)
{
    logger.LogCritical(e, "Exiting application because of ");
}

return 0;
