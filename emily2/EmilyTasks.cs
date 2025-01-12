﻿using emily2.Family;
using emily2.Options;
using Microsoft.Extensions.Logging;

namespace emily2
{
    /// <summary>
    /// UI tasks of the application
    /// </summary>
    internal class EmilyTasks
    {
        internal static ApplicationOptions? CheckOrCreateUser(ApplicationOptions? appSettings, SecretOptions? secretOptions, ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(appSettings);

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
                appSettings.LoadUserSecrets(secretOptions, logger);
            }

            // Write the values to the console.
            Console.WriteLine($"Username: {appSettings.User.UserName} <{appSettings.User.Email}>");
            Console.WriteLine($"{appSettings.User.RSA!.ExportRSAPrivateKeyPem()}");
            Console.WriteLine($"{appSettings.User.RSA!.ExportRSAPublicKeyPem()}");
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

        internal static FamilyProjectManager? CheckOrCreateProject(ApplicationOptions? appSettings, ILoggerFactory logFactory)
        {
            ArgumentNullException.ThrowIfNull(appSettings);

            // initializating project
            bool? openOrCreateProject = null;
            do
            {
                if (string.IsNullOrEmpty(appSettings.LastProjectName))
                {
                    Console.Write("Input family name (empty to exit): ");
                    appSettings.LastProjectName = Console.ReadLine();
                }

                if (string.IsNullOrEmpty(appSettings.LastProjectName))
                {
                    openOrCreateProject = false; // exiting
                }
                else
                {
                    string projectPath = appSettings.GetProjectPathForFamily(appSettings.LastProjectName)!;
                    if (Path.Exists(projectPath))
                    {
                        openOrCreateProject = true;
                    }
                    else
                    {
                        Console.Write($"Family project path ({projectPath}) missing. Create project (y/n)? ");
                        ConsoleKeyInfo createProject = Console.ReadKey();
                        Console.WriteLine();

                        if (char.ToLower(createProject.KeyChar) == 'y')
                        {
                            Directory.CreateDirectory(projectPath);
                            openOrCreateProject = true;
                        }
                        else
                        {
                            // reset family question and ask again
                            appSettings.LastProjectName = null;
                        }
                    }
                }
            }
            while (!openOrCreateProject.HasValue);

            if (openOrCreateProject == true)
            {
                Console.WriteLine($"Opening family's project path: {appSettings.GetProjectPathForFamily(appSettings.LastProjectName)}");
                return appSettings.CreateFamilyProjectManager(logFactory);
            }

            // return null to exit application
            return null;
        }

        internal static void GoMainOperationMenu(FamilyProjectManager familyManager)
        {
            ConsoleKeyInfo operation;
            do
            {
                PrintFamilyMembers(familyManager.Family);
                operation = PrintMainOperationMenu(familyManager.Family);

                // 0 - selected
                if (operation.Key == ConsoleKey.D0 || operation.Key == ConsoleKey.NumPad0)
                {
                    FamilyMember? member = CreateNewFamilyMember();
                    if (member != null)
                    {
                        familyManager.AddNewFamilyMember(member);
                    }
                }
                // 1 - 9 selected
                if ((operation.Key >= ConsoleKey.D1 && operation.Key <= ConsoleKey.D9) ||
                    (operation.Key >= ConsoleKey.NumPad1 && operation.Key <= ConsoleKey.NumPad9))
                {
                    GoFamilyMemberOperationMenu(familyManager, operation);
                }
            }
            while (operation.Key != ConsoleKey.Escape);
        }

        private static void PrintFamilyMembers(Family.Family? family)
        {
            int index = 0;

            foreach (var member in family ?? Enumerable.Empty<Family.FamilyMember>()) 
            {
                // index starts from 1
                Console.WriteLine($"{++index} - {member.UniqueName}");
                Console.WriteLine($"\tGUID:  {member.Id}");
                Console.WriteLine($"\tEMAIL: {member.Email}");
                Console.WriteLine();
            }
        }

        private static ConsoleKeyInfo PrintMainOperationMenu(Family.Family family)
        {
            Console.WriteLine($"{family.Name}'s operation menu:");
            Console.WriteLine($"\t0.   Add new family member");
            Console.WriteLine($"\t1-{family.Count()}. Select family member");
            Console.WriteLine($"\tESC. Exit");
            Console.WriteLine();
            return Console.ReadKey(true);
        }

        private static FamilyMember? CreateNewFamilyMember()
        {
            var member = new FamilyMember();

            Console.Write("Input member's first name: ");
            member.FirstName = Console.ReadLine();
            if (string.IsNullOrEmpty(member.FirstName))
            {
                return null;
            }

            Console.Write("Input member's last name: ");
            member.LastName = Console.ReadLine();

            Console.Write("Input member's e-mail: ");
            member.Email = Console.ReadLine();

            return member;
        }

        private static void GoFamilyMemberOperationMenu(FamilyProjectManager familyManager, ConsoleKeyInfo operation)
        {
            FamilyMember? member = SelectFamilyMember(familyManager.Family, operation);
            if (member == null) return;

            do
            {
                operation = PrintFamilyMemberOperationMenu(member);

            }
            while (operation.Key != ConsoleKey.Escape);
        }

        private static FamilyMember? SelectFamilyMember(Family.Family family, ConsoleKeyInfo operation)
        {
            int index = 0;

            if (operation.Key >= ConsoleKey.D1 && operation.Key <= ConsoleKey.D9)
            {
                index = (int)operation.Key - 49;
            }
            else if (operation.Key >= ConsoleKey.NumPad1 && operation.Key <= ConsoleKey.NumPad9)
            {
                index = (int)operation.Key - 97;
            }

            return family.GetFamilyMemberByIndex(index);
        }

        private static ConsoleKeyInfo PrintFamilyMemberOperationMenu(FamilyMember member)
        {
            Console.WriteLine($"Selected {member.UniqueName}:");

            if (!string.IsNullOrEmpty(member.Email))
            {
                Console.WriteLine($"\tC.   Send your contact information to {member.UniqueName}");
                if (!string.IsNullOrEmpty(member.PublicKey))
                {
                    Console.WriteLine($"\tE.   Send family tree to {member.UniqueName}");
                }
            }

            Console.WriteLine("\tESC. Exit");
            Console.WriteLine();
            return Console.ReadKey(true);
        }

    }
}
