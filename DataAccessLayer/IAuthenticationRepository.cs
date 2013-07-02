using System.Collections.Generic;
using DataDomainEntities;

namespace DataAccessLayer
{
    public interface IAuthenticationRepository
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        void Add(User user);
        void Remove(User user);
        bool ValidateUser(string nickname, string password);
        bool ValidateLogOnUser(string nickname, string password);
        bool ValidateActivationUser(string nickname, string password);
        bool ActivationAccount(string nickname);
        void SetRole(User u, string s);
    }
}