using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emily2.Family
{
    internal class FamilyDuplicatePolicy
    {
#pragma warning disable IDE0060 // unused variable member
        public static FamilyMember? RejectDuplicateNames(FamilyMember member)
        {
            return null;
        }
#pragma warning restore IDE0060

        public static FamilyMember? IncreaseIndexForDuplicateNames(FamilyMember member) 
        {
            member.IncreaseIndex();
            return member; 
        }
    }
}
