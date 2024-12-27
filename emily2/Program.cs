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

{
    mainScope.LogTrace("Reading user options");
    SecretOptions userSecret = config.Get<SecretOptions>();
    ApplicationOptions appSettings = config
        .Get<ApplicationOptions>()?
        .LoadUserSecrets(userSecret);

    // initializating user
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
    Console.WriteLine($"Family's project path: {appSettings.ProjectPath}");

    // initializating project
    bool? openOrCreateProject = null;
    do
    {
        if (string.IsNullOrEmpty(appSettings.ProjectPath))
        {
            Console.Write("Input family name (empty to exit): ");
            appSettings.SetProjectPathForFamily(Console.ReadLine());
        }

        if (string.IsNullOrEmpty(appSettings.ProjectPath))
        {
            openOrCreateProject = false; // exiting
        }
        else 
        {
            if (Path.Exists(appSettings.ProjectPath))
            {
                openOrCreateProject = true;
            }
            else
            {
                Console.Write($"Family project path ({appSettings.ProjectPath}) missing. Create project (y/n)? ");
                ConsoleKeyInfo createProject = Console.ReadKey();
                Console.WriteLine();

                if (char.ToLower(createProject.KeyChar) == 'y')
                {
                    openOrCreateProject = true;
                }
                else
                {
                    // reset family question and ask again
                    appSettings.ProjectPath = null; 
                }
            }
        }
    }
    while (!openOrCreateProject.HasValue);

    // saving settings before opening the project
    // so settings will be saved even if application falls
    appSettings
        .SaveUserSecrets(userSecret)
        .SaveApplicationOptions();

    if (openOrCreateProject == true)
    {
        var family = new Family(appSettings, emily2.Logger.LoggerExtensions.LoggerFactory.CreateLogger<Family>());
    }
}

return 0;
