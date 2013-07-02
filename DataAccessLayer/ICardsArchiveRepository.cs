using System.Collections.Generic;
using DataDomainEntities;

namespace DataAccessLayer
{
    public interface ICardsArchiveRepository
    {
        IEnumerable<Card> GetAll();
        Card GetById(int id);
        IEnumerable<Card> GetAllByBoardId(int idBoard);
        void Add(Card c);
        void Remove(Card c);
    }
}
