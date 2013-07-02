using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataDomainEntities
{
    public class Album
    {
        //public int Id { get; set; }
        //public int IdBoard { get; set; }
        //public int IdList { get; set; }
        //public int IdArchive { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Year {get; set;}
        public IEnumerable<Music> listMusics { get; set; }
      
    }
}
