using System.Collections.Generic;
using System;

namespace DataAccessLayer
{
    public class AuthenticationMemoryLocator
    {
        private readonly static IAuthenticationRepository Repo = new AuthenticationMemoryRepository();
        public static IAuthenticationRepository Get()
        {
            return Repo;
        }
    }
}