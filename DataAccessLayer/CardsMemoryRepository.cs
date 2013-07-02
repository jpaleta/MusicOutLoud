using System.Collections.Generic;
using DataDomainEntities;

namespace DataAccessLayer
{
    public class CardsMemoryRepository : ICardsRepository
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

        public Card GetCardById(int id)
        {
            Card card = null;
            foreach (var card1 in _repo)
            {
                if(card1.Value.Id == id)
                {
                    card = card1.Value;
                    break;
                }
            }
            return card;
        }
        
        public void Add(Card c, int idList, int idBoard)
        {
            c.IdBoard = idBoard;
            c.IdList = idList;
            c.Id = _cid;
            _repo.Add(_cid++, c);
        }


        // iv - 2012.11.11 
        public void Add(string name, int idList, int idBoard)
        {
            Card c = new Card { Id = _cid, IdBoard = idBoard, IdList = idList, Name = name };
            _repo.Add(_cid++, c);
        }

        // iv - 2012.11.11
        public int getCountByBoard(int listId, int boardId)
        {
            int count = 0;
            foreach (var card in _repo)
            {
                if (card.Value.IdList == listId && card.Value.IdBoard == boardId)
                {
                    count++;
                }
            }
            return count;
        }

        //iv - 2012.11.11
        /*public void Remove(int id)
        {
            if (_repo.Count > 0)
            {
                foreach (var card in _repo)
                {
                    if (card.Value.Id == id)
                    {
                        _repo.Remove(card.Key);
                        break;
                    }
                }
            }
        }*/

        // 2012.10.19 - João
        //remove a lista de um board

        public void Remove(Card c)
        {
            _repo.Remove(c.Id);
        }

        // obtem os cards de uma determinada lista
        public IEnumerable<Card> GetAllByListAndBoardId(int idList, int idBoard)
        {
            IDictionary<int, Card> _tmp = new Dictionary<int, Card>();
            int _kid = 0;

            foreach (KeyValuePair<int, Card> kvp in _repo)
            {
                if (kvp.Value.IdList == idList && kvp.Value.IdBoard == idBoard)
                {
                    _tmp.Add(_kid++, kvp.Value);
                }
            }
            return _tmp.Values;
        }
        
        //2012.10.22 - david
        //altera posição das cards entra A e B
        public void Swap(int A, int B)
        {
            var tmpN = _repo[A].Name;
            var tmpCD = _repo[A].CreationDate;
            var tmpCLD = _repo[A].ConclusionLimitDate;
            var tmpDesc = _repo[A].Description;

            _repo[A].Name = _repo[B].Name;
            _repo[A].CreationDate = _repo[B].CreationDate;
            _repo[A].ConclusionLimitDate = _repo[B].ConclusionLimitDate;
            _repo[A].Description = _repo[B].Description;

            _repo[B].Name = tmpN;
            _repo[B].CreationDate = tmpCD;
            _repo[B].ConclusionLimitDate = tmpCLD;
            _repo[B].Description = tmpDesc;

        }

    }
}
