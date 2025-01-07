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

        public bool AddFamilyMember(FamilyMember? member)
        {
            logger.LogTraceMethod();

            if (member == null) return false;

            // check duplicate by email
            if (!string.IsNullOrEmpty(member.Email))
            {
                var existingMember = _members.FirstOrDefault(m => m.Email == member.Email);
                if (existingMember != null)
                {
                    logger.LogWarning("Not adding new family member ({member.Name}), because existing member ({existingMember.Name}) has same email ({existingMember.Email})",
                        member.Name,
                        existingMember.Name,
                        existingMember.Email);
                    return false;
                }
            }

            // check duplicate by Id
            {
                var existingMember = _members.FirstOrDefault(m => m.Id == member.Id);
                if (existingMember != null)
                {
                    logger.LogWarning("Not adding new family member ({member.Name}), because existing member ({existingMember.Name}) has same id ({existingMember.Id})",
                        member.Name,
                        existingMember.Name,
                        existingMember.Email);
                    return false;
                }
            }

            logger.LogTrace("Adding member ({member.Id} - {member.Name}) to family", member.Id, member.Name);
            _members.Add(member);
            return true;
        }
    }
}
