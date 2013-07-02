using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web;

namespace DataDomainEntities
{
    public class PlayList
    {
        public int Id { get; set; }
        //[Required]
        public string Name { get; set; } // Nome único
        public string Description { get; set; }
        // iv - 2012.11.11
        public List<Music> Lists;
        // iv - 2012.11.17
        public string Owner;
        public IList<string> AuthorisedUsersRead;
        public IList<string> AuthorisedUsersWrite;

        public bool toShare;

        public bool IsSharedWith(string name)
        {
            return ((this.AuthorisedUsersRead != null && this.AuthorisedUsersRead.Contains(name)) || (this.AuthorisedUsersWrite != null && this.AuthorisedUsersWrite.Contains(name)));
        }
    }
}