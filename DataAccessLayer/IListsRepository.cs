using System.Collections.Generic;
using DataDomainEntities;

namespace DataAccessLayer
{
    public interface IListsRepository  
    {
        //new 14-10-2012
        IEnumerable<BList> GetAll();
        BList GetById(int id);
        /*BList GetById(int idBoard);
        BList GetById(int idBoard, int id);*/
        void Add(BList l, int idBoard);
        void Add(BList l, string BoardN);
        // iv - 2012.11.11
        void Add(string name, int idBoard);
        // 2012.10.18 - idálio
        // novo método implementado
        IEnumerable<BList> GetAllByBoardId(int idBoard);

        void Swap(int p, int id2);

        void Remove(IEnumerable<BList> lists, int id);
        
        //iv - 2012.11.11
        //void Remove(int id);
        int getCountByBoard(int boardId);
        bool VerifyUniqueName(string name, int boardId);
        BList GetListById(int id);
        // iv - 2012.11.13
        void Remove(BList l);
    }
}