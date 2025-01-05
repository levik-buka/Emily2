using emily2.Family;
using emily2.Logger;
using emily2.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emily2.Repository
{
    internal interface IFamilyRepository
    {
        protected ILogger Logger { get; }
        protected ApplicationOptions AppSettings { get; }

        /// <summary>
        /// Loads members to the family
        /// </summary>
        /// <param name="family"></param>
        /// <returns>number of loaded family members</returns>
        abstract public int LoadFamilyMembers(Family.Family family);

        abstract void SaveFamilyMember(FamilyMember member);

        public bool AddUserToFamilyMembers(Family.Family family)
        {
            using var logScope = Logger.BeginMethodScope();

            if (string.IsNullOrEmpty(AppSettings.User?.Email))
            {
                Logger.LogError("User ({username}) can not be added to family, because it missing email", AppSettings.User?.UserName);
                return false;
            }

            var user = new FamilyMember
            {
                FirstName = AppSettings.User.UserName,
                LastName = family.Name,
                Email = AppSettings.User.Email
            };

            if (family.AddFamilyMember(user))
            {
                SaveFamilyMember(user);
                return true;
            }

            return false;
        }
    }
}
