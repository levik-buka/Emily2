﻿using emily2.Options;
using Microsoft.Extensions.Logging;

namespace emily2
{
    internal class EmilyTasks()
    {
        internal static ApplicationOptions CheckOrCreateUser(ApplicationOptions appSettings, SecretOptions secretOptions)
        {
            if (appSettings == null) throw new ArgumentNullException(nameof(appSettings));

            bool newUser = false;

            // initializating user
            if (string.IsNullOrEmpty(appSettings.User?.Email))
            {
                newUser = true;
                appSettings.User = new UserOptions();

                Console.Write("Input user's name: ");
                appSettings.User.UserName = Console.ReadLine();

                Console.Write("Input user's e-mail: ");
                appSettings.User.Email = Console.ReadLine();

                // create new RSA key for the user
                appSettings.LoadUserSecrets(secretOptions);
            }

            // Write the values to the console.
            Console.WriteLine($"Username: {appSettings.User.UserName} <{appSettings.User.Email}>");
            Console.WriteLine($"{appSettings.User.RSA.ExportRSAPrivateKeyPem()}");
            Console.WriteLine($"{appSettings.User.RSA.ExportRSAPublicKeyPem()}");
            Console.WriteLine($"Secret container: {appSettings.SecretContainer}");

            if (newUser)
            {
                Console.Write($"Save user (y/n)? ");
                ConsoleKeyInfo createProject = Console.ReadKey();
                Console.WriteLine();

                if (char.ToLower(createProject.KeyChar) == 'y')
                {
                    // continue with new user
                    return appSettings;
                }

                // return null to exit application
                return null;
            }

            // continue with existing user
            return appSettings;
        }

        internal static Family.Family CheckOrCreateProject(ApplicationOptions appSettings)
        {
            if (appSettings == null) throw new ArgumentNullException(nameof(appSettings));

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

            if (openOrCreateProject == true)
            {
                Console.WriteLine($"Opening family's project path: {appSettings.ProjectPath}");
                return new Family.Family(appSettings, Logger.LoggerExtensions.LoggerFactory.CreateLogger<Family.Family>());
            }

            // return null to exit application
            return null;
        }
    }
}