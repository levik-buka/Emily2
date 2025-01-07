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
            projectPath = Path.TrimEndingDirectorySeparator(projectPath);
            string dirName = member.GetFamilyMemberDirectoryName();
            return $"{projectPath}\\{dirName}\\{dirName}.json";
        }

        private static string GetFamilyMemberDirectoryName(this FamilyMember member)
        {
            string directoryName = string.Join(' ', [member.LastName, member.FirstName, member.Index]).Trim();

            if (string.IsNullOrEmpty(directoryName))
            {
                throw new InvalidOperationException($"Can not get family member's directory name because of missing name ({member.Name})");
            }

            return directoryName;
        }

        public static string GetFamilyMemberJsonFilename(this DirectoryInfo memberDir)
        {
            string memberPath = Path.TrimEndingDirectorySeparator(memberDir.FullName);
            return $"{memberPath}\\{memberDir.Name}.json";
        }
    }
}
