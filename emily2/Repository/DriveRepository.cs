using emily2.Family;
using emily2.Logger;
using emily2.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace emily2.Repository
{
    internal class DriveRepository(ApplicationOptions appSettings, ILogger<DriveRepository> logger) : IFamilyRepository
    {
        ILogger IFamilyRepository.Logger => logger;

        ApplicationOptions IFamilyRepository.AppSettings => appSettings;

        public int LoadFamilyMembers(Family.Family family)
        {
            using var logScope = logger.BeginMethodScope();

            int subDirectoryCount = 0;
            int addedMembersCount = 0;

            var familyDirectory = new DirectoryInfo(appSettings.ProjectPath!);
            foreach (var subDirectory in familyDirectory.EnumerateDirectories())
            {
                subDirectoryCount++;

                var familyMember = new FamilyMember();
                if (family.AddFamilyMember(familyMember))
                {
                    addedMembersCount++;
                }
            }

            logScope.LogTrace($"Loaded {addedMembersCount}/{subDirectoryCount} family members");
            return addedMembersCount;
        }

        public void SaveFamilyMember(FamilyMember member)
        {
            using var logScope = logger.BeginMethodScope();
        }
    }
}
