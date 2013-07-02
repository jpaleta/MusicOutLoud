using System.Collections.Generic;
using DataDomainEntities;

namespace DataAccessLayer
{
    public class CardsArchiveMemoryRepository : ICardsArchiveRepository
    {

        private readonly IDictionary<int, Card> _repo = new Dictionary<int, Card>();
        private int _cid = 0;

        public IEnumerable<Card> GetAll()
        {
            return _repo.Values;
        }

        public Card GetById(int id)
        {
            Card card = null;
            _repo.TryGetValue(id, out card);
            return card;
        }

        public IEnumerable<Card> GetAllByBoardId(int idBoard)
        {
            IDictionary<int, Card> _tmp = new Dictionary<int, Card>();
            int _kid = 0;

            foreach (KeyValuePair<int, Card> kvp in _repo)
            {
                if (kvp.Value.IdBoard == idBoard)
                {
                    _tmp.Add(_kid++, kvp.Value);
                }
            }
            return _tmp.Values;
        }

        public void Add(Card c)
        {
            c.IdArchive = _cid;
            _repo.Add(_cid++, c);
        }

        public void Remove(Card c)
        {
            _repo.Remove(c.IdArchive);
        }
    }
}

