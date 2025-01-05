using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emily2.Family
{
    internal class FamilyMember
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string Name 
        { 
            get
            {
                return string.Join(' ', [ FirstName, LastName ]);
            }
        }

        public string? Email { get; set; }
    }
}
