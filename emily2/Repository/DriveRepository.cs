using emily2.Family;
using emily2.Logger;
using emily2.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace emily2.Repository
{
    public class DriveRepository(string projectPath, ILogger<DriveRepository> logger) : IFamilyRepository
    {
        private readonly JsonSerializerOptions JSON_SERIALIZATION_OPTIONS = new() { WriteIndented = true };

        public int LoadFamilyMembers(Family.Family family)
        {
            using var logScope = logger.BeginMethodScope();
            ArgumentException.ThrowIfNullOrEmpty(projectPath);

            int subDirectoryCount = 0;
            int addedMembersCount = 0;

            var familyDirectory = new DirectoryInfo(projectPath);
            foreach (var subDirectory in familyDirectory.EnumerateDirectories())
            {
                subDirectoryCount++;

                try
                {
                    string memberJson = File.ReadAllText(subDirectory.GetFamilyMemberJsonFilename());
                    var familyMember = JsonSerializer.Deserialize<FamilyMember>(memberJson);

                    if (family.AddFamilyMember(familyMember))
                    {
                        addedMembersCount++;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Failed to load family member's file. ");
                }
            }

            logScope.LogTrace($"Loaded {addedMembersCount}/{subDirectoryCount} family members");
            return addedMembersCount;
        }

        public void SaveFamilyMember(FamilyMember member)
        {
            using var logScope = logger.BeginMethodScope(member.Name);

            string memberPath = member.GetFamilyMemberJsonFilename(projectPath);
            logScope.LogTrace("Saving family member to {path}", memberPath);

            // create member directory if missing
            Directory.CreateDirectory(Path.GetDirectoryName(memberPath)!);

            var memberJson = JsonSerializer.Serialize(member, JSON_SERIALIZATION_OPTIONS);
            File.WriteAllText(memberPath, memberJson);
        }

    }
}
