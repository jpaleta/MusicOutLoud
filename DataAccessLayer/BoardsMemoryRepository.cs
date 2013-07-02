using System.Collections.Generic;
using DataDomainEntities;

namespace DataAccessLayer
{
    

    public class BoardsMemoryRepository : IBoardsRepository
    {
        private readonly IDictionary<int, Board> _repo = new Dictionary<int, Board>();
        private int _cid = 0;

        public IEnumerable<Board> GetAll()
        {
            return _repo.Values;
        }

        public Board GetById(int id)
        {
            Board td = null;
            _repo.TryGetValue(id, out td);
            return td;
        }

        public void Add(Board td)
        {
            td.Id = _cid;
            _repo.Add(_cid++,td);
        }

        public void Remove(Board td)
        {  
            _repo.Remove(td.Id);
        }

        public bool VerifyUniqueName(string name)
        {
            foreach (Board b in this.GetAll())
            {
                if (b.Name.Equals(name))
                    return false;
            }

            return true;
        }

        public void SetUserPermition(int boardId, string nickname, Permition perm)
        {
            Board b = null;
            _repo.TryGetValue(boardId, out b);
            int position;
            switch (perm)
            {
                case Permition.Select:
                    break;
                case Permition.None:
                    if (b.AuthorisedUsersWrite != null && b.AuthorisedUsersWrite.Contains(nickname))
                    {
                        position = b.AuthorisedUsersWrite.IndexOf(nickname);
                        b.AuthorisedUsersWrite.RemoveAt(position);
                    }
                    if (b.AuthorisedUsersRead != null && b.AuthorisedUsersRead.Contains(nickname))
                    {
                        position = b.AuthorisedUsersRead.IndexOf(nickname);
                        b.AuthorisedUsersRead.RemoveAt(position);
                    }
                    break;
                case Permition.Reader:
                    if (b.AuthorisedUsersRead == null)  b.AuthorisedUsersRead = new List<string>();
                    if (b.AuthorisedUsersRead.Contains(nickname) == false)  b.AuthorisedUsersRead.Add(nickname);
                    if (b.AuthorisedUsersWrite != null && b.AuthorisedUsersWrite.Contains(nickname))
                    {
                        position = b.AuthorisedUsersWrite.IndexOf(nickname);
                        b.AuthorisedUsersWrite.RemoveAt(position);
                    }
                    break;
                case Permition.Writer:
                    if (b.AuthorisedUsersWrite == null) b.AuthorisedUsersWrite = new List<string>(); 
                    if (b.AuthorisedUsersWrite.Contains(nickname) == false) b.AuthorisedUsersWrite.Add(nickname);
                    if (b.AuthorisedUsersRead != null && b.AuthorisedUsersRead.Contains(nickname))
                    {
                        position = b.AuthorisedUsersRead.IndexOf(nickname);
                        b.AuthorisedUsersRead.RemoveAt(position);
                    }
                    break;
            }
        }

        public bool GetBoardByName(string name) 
        {
            //IEnumerable<Board> boards = GetAll();

            foreach (Board b in this.GetAll()) 
            {
                if (b.Name.Equals(name))
                    return false;
            }

            return true;
        }
    }
}