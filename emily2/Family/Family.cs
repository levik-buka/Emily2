using emily2.Logger;
using Microsoft.Extensions.Logging;
using System.Collections;

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

        public FamilyMember? AddFamilyMember(FamilyMember? member, Func<FamilyMember, bool> duplicatePolicy)
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
                        member.Name,
                        existingMember.Name,
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
                        member.Name,
                        existingMember.Name,
                        existingMember.Email);
                    return null;
                }
            }

            // check duplicate by Name and Index
            {
                var existingMember = _members.FirstOrDefault(m => (m.Name == member.Name && m.Index == member.Index));
                if (existingMember != null && duplicatePolicy(member) == true)
                {
                    // duplicate policy changed member's data, so let's try to add again
                    return AddFamilyMember(member, duplicatePolicy);
                }
            }

            logger.LogTrace("Adding member ({member.Id} - {member.Name}/index: {member.Index}) to family", member.Id, member.Name, member.Index);
            _members.Add(member);
            return member;
        }
    }
}
