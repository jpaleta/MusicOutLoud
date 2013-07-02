using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDomainEntities
{
    public class BList
    {
        
        public int Id { get; set; }
        public int IdBoard { get; set; }
        public int IdArchive { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }  // na implementação o Description deve passar a name
        // iv - 2012.11.11
        public IEnumerable<Card> Cards;

        // iv - 2012.11.17
        public string Owner;
        public IEnumerable<string> AuthorisedUsersRead;
        public IEnumerable<string> AuthorisedUsersWrite;

        

    }
}
