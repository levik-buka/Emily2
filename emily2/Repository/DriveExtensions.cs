using emily2.Family;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emily2.Repository
{
    public static class DriveExtensions
    {
        public static string GetFamilyMemberJsonFilename(this FamilyMember member, string projectPath)
        {
            string memberName = string.Join(' ', [member.LastName, member.FirstName, member.Index]).Trim();
            if (string.IsNullOrEmpty(memberName))
            {
                throw new InvalidOperationException($"Can not get family member's filename because of missing name ({member.Name})");
            }

            projectPath = Path.TrimEndingDirectorySeparator(projectPath);
            return $"{projectPath}\\{memberName}\\{memberName}.json";
        }

        public static string GetFamilyMemberJsonFilename(this DirectoryInfo memberDir)
        {
            string memberPath = Path.TrimEndingDirectorySeparator(memberDir.FullName);
            return $"{memberPath}\\{memberDir.Name}.json";
        }
    }
}
