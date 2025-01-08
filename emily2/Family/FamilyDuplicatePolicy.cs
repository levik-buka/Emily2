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
        public static bool RejectDuplicate(FamilyMember member)
        {
            return false;
        }
#pragma warning restore IDE0060

        public static bool IncreaseIndexForDuplicate(FamilyMember member) 
        {
            member.IncreaseIndex();
            return true; 
        }
    }
}
