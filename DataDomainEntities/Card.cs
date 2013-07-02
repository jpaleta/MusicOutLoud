using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataDomainEntities
{
    public class Card
    {
        public int Id { get; set; }
        public int IdBoard { get; set; }
        public int IdList { get; set; }
        public int IdArchive { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate {get; set;}
        public DateTime ConclusionLimitDate {get; set;}
        public IEnumerable<SelectListItem> listOfListsInBoard { get; set; }
        public string SelectedValue { get; set; }

        // iv - 2012.11.17
        public string Owner;
        public IEnumerable<string> AuthorisedUsersRead;
        public IEnumerable<string> AuthorisedUsersWrite;

        
    }
}
