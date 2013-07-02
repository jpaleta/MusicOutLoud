using System.Collections.Generic;
using DataDomainEntities;

namespace DataAccessLayer
{
    public class AuthenticationMemoryRepository : IAuthenticationRepository
    {
        private static readonly IDictionary<int, User> _repo = new Dictionary<int, User>();
        private int _cid = 0;


        public AuthenticationMemoryRepository() {
            this.Add(new User { Nickname = "Administrator", Nome = "Administrator", Password = "12345", Email = "bizk0it0k0x0@gmail.com", role = 0, Active = true});
        }

        public IEnumerable<User> GetAll()
        {
            return _repo.Values;
        }

        public User GetById(int id)
        {
            User user = null;
            _repo.TryGetValue(id, out user);
            return user;
        }

        public void Add(User user)
        {
            user.Uid = _cid;
            _repo.Add(_cid++,user);
        }

        public void Remove(User user)
        {  
            _repo.Remove(user.Uid);
        }

        public void SetRole(User u, string s)
        {
            Role result;
            Role.TryParse(s, true, out result);
            u.role = result;
            if (result == Role.Anonimous)
                u.Active = false;
        }

        public bool ValidateUser(string nickname, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }
            else
            {
                foreach (KeyValuePair<int, User> kvp in _repo)
                {
                    if (kvp.Value.Nickname == nickname)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool ValidateLogOnUser(string nickname, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }
            else
            {
                foreach (KeyValuePair<int, User> kvp in _repo)
                {
                    if (kvp.Value.Nickname == nickname && kvp.Value.Password == password)
                    {
                        //dm - 2012.11.19 - duas verificações, se o user existe 
                        return true;

                        //// iv - 2012.11.17 - só se pode loggar se tiver registo activo
                        //if (kvp.Value.Active)
                        //{
                        //    return true;
                        //}
                    }
                }
            }
            return false;
        }

        public bool ValidateActivationUser(string nickname, string password)
        {
            foreach (KeyValuePair<int, User> kvp in _repo)
            {
                if (kvp.Value.Nickname == nickname && kvp.Value.Password == password)
                {
                    // dm - 2012.11.19 - verifica  se o user está activado
                    if (kvp.Value.Active)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ActivationAccount(string nickname) {
            if (string.IsNullOrEmpty(nickname))
            {
                return false;
            }
            else {
                foreach (KeyValuePair<int, User> kvp in _repo)
                {
                    if (kvp.Value.Nickname == nickname)
                    {
                        kvp.Value.Active = true;                        
                        return true;
                    }
                }
            }
            return false;
        }
    }
}