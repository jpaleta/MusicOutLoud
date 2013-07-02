using System.Collections.Generic;
using DataDomainEntities;

namespace DataAccessLayer
{
    

    public interface IPlayListRepository  
    {
        IEnumerable<PlayList> GetAll();
        PlayList GetById(int id);
        void Add(PlayList td);
        void Remove(PlayList td);
        PlayList GetPlayListByName(string name);
        bool VerifyUniqueName(string name);
        bool GetPlayListByNameAndUser(User user, string name);

        void SetUserPermition(int playListId, string nickname, Permition perm);
    }
}