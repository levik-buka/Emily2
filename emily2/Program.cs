﻿using emily2.Logger;
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
    UserOptions? userOptions = config
        .GetSection("User")?
        .Get<UserOptions>()?
        .LoadUserRSA();

    // Write the values to the console.
    Console.WriteLine($"Username: {userOptions?.UserName} <{userOptions?.Email}>");
    Console.WriteLine($"Private Key = {userOptions?.RSA.ExportRSAPrivateKeyPem()}");
    Console.WriteLine($"Public Key  = {userOptions?.RSA.ExportRSAPublicKeyPem()}");

    var appOptions = new ApplicationOptions(userOptions);
    appOptions.SaveApplicationOptions();
}
