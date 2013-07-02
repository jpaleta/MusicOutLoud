using System.Collections.Generic;
using DataDomainEntities;

namespace DataAccessLayer
{

    public class ListsMemoryRepository : IListsRepository
    {
        private readonly IDictionary<int, BList> _repo = new Dictionary<int, BList>();
        private int _cid = 0;

        public IEnumerable<BList> GetAll()
        {
            return _repo.Values;
        }
        /*public BList GetById(int idBoard)
        {
            BList list = null;
            _repo.TryGetValue(idBoard, out list);
            return list;
        }*/
        public BList GetById(int id)
        {
            BList list = null;
            _repo.TryGetValue(id, out list);
            return list;
        }

        /*public BList GetById(int idBoard, int id)
        {
            BList list = null;
            _repo.TryGetValue(id, out list);
            return list;
        }*/

        public void Add(BList l, int idBoard)
        {
            l.IdBoard = idBoard;
            l.Id = _cid;
            _repo.Add(_cid++,l);
        }
        //2012.10.21 - David
        public void Add(BList l, string BoardN)
        {
            l.Id = _cid;
            _repo.Add(_cid++, l);
        }

        // iv - 2012.11.11 
        public void Add(string name, int idBoard)
        {
            BList l = new BList { Id = _cid, IdBoard = idBoard, Name = name };
            _repo.Add(_cid++, l);
        }

        public BList GetListById(int id)
        {
            BList list = null;
            foreach (var bList in _repo)
            {
                if (bList.Value.Id == id)
                {
                    list = bList.Value;
                    break;
                }
            }
            return list;
        }

        // 2012.10.19 - João
        //remove a lista de um board

        public void Remove(IEnumerable<BList> l, int id)
        {
            foreach (BList list in l)
            {
                 if (list.Id == id)
                    _repo.Remove(id);
            }

        }

        //iv - 2012.11.11
        /*public void Remove(int id)
        {
            if (_repo.Count > 0)
            {
                foreach (var bList in _repo)
                {
                    if (bList.Value.Id == id)
                    {
                        _repo.Remove(bList.Key);
                        break;
                    }
                }
            }
           
            //_repo.Remove(id);
        }*/

        // iv - 2012.11.13
        public void Remove(BList l)
        {
            _repo.Remove(l.Id);
        }

        // iv - 2012.11.11
        public int getCountByBoard(int boardId)
        {
            int count = 0;
            foreach (var bList in _repo)
            {
                if (bList.Value.IdBoard == boardId)
                {
                    count++;
                }
            }
            return count;
        }

        // 2012.10.18 - Idálio
        // obtem as listas de um determinado board
        public IEnumerable<BList> GetAllByBoardId(int idBoard) { 
            IDictionary<int, BList> _tmp = new Dictionary<int, BList>();
            int _kid = 0;
            
            foreach(KeyValuePair<int, BList> kvp in _repo){
                if (kvp.Value.IdBoard == idBoard) { 
                    _tmp.Add(_kid++, kvp.Value);
                }
            }
            return _tmp.Values;
        }

        // 2012.10.20 - David
        //Faz swap de duas listas.
        public void Swap(int A, int B)
        {
            var tmpN = _repo[A].Name;
            var tmpIdA = _repo[A].IdArchive;
            var tmpD = _repo[A].Description;
            _repo[A].Name = _repo[B].Name;
            _repo[A].IdArchive = _repo[B].IdArchive;
            _repo[A].Description = _repo[B].Description;
            _repo[B].Name = tmpN;
            _repo[B].IdArchive = tmpIdA;
            _repo[B].Description = tmpD;
        }

        public bool VerifyUniqueName(string name, int boardId)
        {
            foreach (BList b in this.GetAll())
            {
                if (b.Name.Equals(name) && b.IdBoard.Equals(boardId))
                    return false;
            }
            return true;
        }

    }
    
}