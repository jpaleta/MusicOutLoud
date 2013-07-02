using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
//using DataDomainSecurity;

namespace DataDomainEntities
{
    public enum Role
    {
        Select = 0,
        Administrator = 1,
        Registered = 2,
        Anonimous = 3
    };

    public enum Permition
    {
        Select = 0,
        None = 1,
        Reader = 2,
        Writer = 3,
    };

    public class User
    {
        public int Uid { get; set; }

        //[Required]
        public string Nickname { get; set; } // Nome único
        //[Required]
        public string Password { get; set; }
        // //DataDomainSecurity.RijndaelSimple.Encrypt(Password, "Pas5pr@se", "@1tValue", "SHA1", 2, "@1B2c3D4e5F6g7H8", 256); 
        //[Required]
        public string Nome { get; set; }
        //[Required]
        public string Email { get; set; }
        //[Required]
        //public DataDomainSecurity.Roles.Role role { get; set; }
        public Permition permition { get; set; }


        public Role role { get; set; }

        public bool Active { get; set; }

        public string Photo { get; set; }

        //public IEnumerable<SelectListItem> listOfRoles { get; set; }

        public Permition GetPermition(PlayList b)
        {
            Permition p = new Permition();
           

            if (!b.IsSharedWith(this.Nickname))
            {
                p = Permition.None;
            }

            if (b.AuthorisedUsersRead != null)
            {
                if (b.AuthorisedUsersRead.Contains(this.Nickname))
                    p = Permition.Reader;
            }
                
            if (b.AuthorisedUsersWrite != null)
            {
                if (b.AuthorisedUsersWrite.Contains(this.Nickname))
                    p = Permition.Writer;
            }

            return p;
        }


    }
}