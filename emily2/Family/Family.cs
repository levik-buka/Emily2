using emily2.Logger;
using emily2.Options;
using emily2.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emily2.Family
{
    public class Family(string name, ILogger<Family> logger) : IEnumerable<FamilyMember>
    {
        private readonly List<FamilyMember> _members = [];

        public string Name { get; set; } = name;

        public IEnumerator<FamilyMember> GetEnumerator()
        {
            return ((IEnumerable<FamilyMember>)_members).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_members).GetEnumerator();
        }

        public FamilyMember? AddFamilyMember(FamilyMember? member, Func<FamilyMember, FamilyMember?> duplicateNamePolicy)
        {
            logger.LogTraceMethod();

            if (member == null) return null;

            // check duplicate by email
            if (!string.IsNullOrEmpty(member.Email))
            {
                var existingMember = _members.FirstOrDefault(m => m.Email == member.Email);
                if (existingMember != null)
                {
                    logger.LogWarning("Not adding new family member ({member.Name}), because existing member ({existingMember.Name}) has same email ({existingMember.Email})",
                        member.UniqueName,
                        existingMember.UniqueName,
                        existingMember.Email);
                    return null;
                }
            }

            // check duplicate by Id
            {
                var existingMember = _members.FirstOrDefault(m => m.Id == member.Id);
                if (existingMember != null)
                {
                    logger.LogWarning("Not adding new family member ({member.Name}), because existing member ({existingMember.Name}) has same id ({existingMember.Id})",
                        member.UniqueName,
                        existingMember.UniqueName,
                        existingMember.Email);
                    return null;
                }
            }

            // check duplicate by Name
            {
                var existingMember = _members.FirstOrDefault(m => m.UniqueName == member.UniqueName);
                if (existingMember != null)
                {
                    var newMember = duplicateNamePolicy(member);
                    if (newMember == null)
                    {
                        logger.LogWarning("Not adding new family member ({member.Name}), because same named already exists in family",
                            member.UniqueName);
                        return null;
                    }

                    return AddFamilyMember(newMember, duplicateNamePolicy);
                }
            }

            logger.LogTrace("Adding member ({member.Id} - {member.Name}) to family", member.Id, member.UniqueName);
            _members.Add(member);
            return member;
        }
    }
}
