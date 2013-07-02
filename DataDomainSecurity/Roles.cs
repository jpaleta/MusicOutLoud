using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDomainSecurity
{
    public class Roles
    {
        public enum Role { 
            Administrator = 1,
            Registered = 2,
            Anonimous = 3
        };
    }
}
