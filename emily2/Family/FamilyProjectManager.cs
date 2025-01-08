﻿using emily2.Logger;
using emily2.Options;
using emily2.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emily2.Family
{
    /// <summary>
    /// Glue class between business logic and repository
    /// </summary>
    /// <param name="family"></param>
    /// <param name="repository"></param>
    /// <param name="logFactory"></param>
    public class FamilyProjectManager(Family family, IFamilyRepository repository, ILoggerFactory logFactory)
    {
        private readonly ILogger _logger = logFactory.CreateClassLogger();

        public Family Family { get => family; }

        public bool AddUserToFamilyMembers(ApplicationOptions appSettings)
        {
            using var logScope = _logger.BeginMethodScope();

            if (string.IsNullOrEmpty(appSettings.User?.Email))
            {
                _logger.LogError("User ({username}) can not be added to family, because it missing email", appSettings.User?.UserName);
                return false;
            }

            var user = new FamilyMember
            {
                FirstName = appSettings.User.UserName,
                LastName = family.Name,
                Email = appSettings.User.Email
            };

            var newMember = family.AddFamilyMember(user, FamilyDuplicatePolicy.RejectDuplicate);
            if (newMember != null)
            {
                repository.SaveFamilyMember(newMember);
                return true;
            }

            return false;
        }

        public int LoadFamilyMembers()
        {
            return repository.LoadFamilyMembers(family);
        }

        internal bool AddNewFamilyMember(FamilyMember member)
        {
            var newMember = family.AddFamilyMember(member, FamilyDuplicatePolicy.IncreaseIndexForDuplicate);
            if (newMember != null)
            {
                repository.SaveFamilyMember(newMember);
                return true;
            }

            return false;
        }
    }
}
