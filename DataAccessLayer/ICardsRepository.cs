using System.Collections.Generic;
using DataDomainEntities;

namespace DataAccessLayer
{
    public interface ICardsRepository
    {
        IEnumerable<Card> GetAll();
        Card GetById(int id);
        void Add(Card c, int idList, int idBoard);
        IEnumerable<Card> GetAllByListAndBoardId(int idList, int idBoard);
        void Swap(int A, int B);
        void Remove(Card c);

        // iv - 2012.11.11
        void Add(string name, int idList, int idBoard);
        int getCountByBoard(int listId, int boardId);
        Card GetCardById(int id);
        //void Remove(int id);
    }
}
