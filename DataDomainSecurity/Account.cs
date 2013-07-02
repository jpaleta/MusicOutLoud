using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDomainEntities;
using DataAccessLayer;

namespace DataDomainSecurity
{
    public class Account
    {
        //private readonly IAuthenticationRepository _repoAuthentication;
        public bool ValidateUser(string username, string password) {
            IAuthenticationRepository _repoAuthentication;
            _repoAuthentication = AuthenticationMemoryLocator.Get();
            return _repoAuthentication.ValidateUser(username, password);
            //return DataAccessLayer.AuthenticationMemoryRepository.ValidateUser(username, password);
        }
    }
}
