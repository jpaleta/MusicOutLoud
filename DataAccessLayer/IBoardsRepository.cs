using System.Collections.Generic;
using DataDomainEntities;

namespace DataAccessLayer
{
    

    public interface IBoardsRepository  
    {
        IEnumerable<Board> GetAll();
        Board GetById(int id);
        void Add(Board td);
        void Remove(Board td);
        bool GetBoardByName(string name);
        bool VerifyUniqueName(string name);

        void SetUserPermition(int boardId, string nickname, Permition perm) ;
    }
}