using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace emily2.Family
{
    public class FamilyMember
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// To separate members with same first and last names, index will be increased
        /// By default index is null. First duplicate has index = 1, second index = 2 and so.
        /// </summary>
        public uint? Index { get; set; }
        [JsonRequired]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [JsonIgnore]
        public string Name 
        { 
            get
            {
                return string.Join(' ', [ FirstName, LastName ]);
            }
        }

        public string? Email { get; set; }

        public void IncreaseIndex()
        {
            if (Index != null) { Index++; }
            else { Index = 1; }
        }
    }
}
