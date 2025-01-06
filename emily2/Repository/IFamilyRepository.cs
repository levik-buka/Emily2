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
        /// <summary>
        /// Loads members to the family
        /// </summary>
        /// <param name="family"></param>
        /// <returns>number of loaded family members</returns>
        abstract public int LoadFamilyMembers(Family.Family family  );

        abstract void SaveFamilyMember(FamilyMember member);
    }
}
