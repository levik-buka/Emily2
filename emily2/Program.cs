using emily2.Options;
using Microsoft.Extensions.Configuration;

IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .Build();

UserOptions userOptions = config.GetRequiredSection("User").Get<UserOptions>();

// Write the values to the console.
Console.WriteLine($"KeyOne = {userOptions.UserName}");
Console.WriteLine($"KeyTwo = {userOptions.Email}");
Console.WriteLine($"PrivateKey = {userOptions.PrivateKey}");

userOptions.PrivateKey = Guid.NewGuid().ToString();
Console.WriteLine($"New PrivateKey = {userOptions.PrivateKey}");

var secretOptions = new SecretOptions(userOptions);
secretOptions.SaveSecretOptions();

