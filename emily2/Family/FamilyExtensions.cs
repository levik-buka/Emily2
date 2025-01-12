using emily2.Logger;
using emily2.Options;
using emily2.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace emily2.Family
{
    internal static class FamilyExtensions
    {
        internal static FamilyProjectManager CreateFamilyProjectManager(this ApplicationOptions appSettings, ILoggerFactory logFactory)
        {
            ArgumentNullException.ThrowIfNull(appSettings);

            var family = new Family(appSettings.LastProjectName!, logFactory.CreateLogger<Family>());

            // here can be created any FamilyRepository based on application options
            // but currently only drive repository is supported
            IFamilyRepository repository = new DriveRepository(appSettings.GetProjectPathForFamily(appSettings.LastProjectName)!, logFactory.CreateLogger<DriveRepository>());
            
            return new FamilyProjectManager(family, repository, logFactory);
        }

        internal static FamilyMember? GetFamilyMemberByIndex(this Family family, int index) 
        { 
            ArgumentNullException.ThrowIfNull(family);
            if (index <= 0 && index > family.Count()) return null;

            return family.ElementAt(index);
        }

        internal static string Pronounce(this FamilyMember member)
        {
            switch (member.Gender)
            {
                case Gender.Male: return "his";
                case Gender.Female: return "her";
                default: return "their";
            }
        }
    }
}
