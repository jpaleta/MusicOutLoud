using System.Collections.Generic;
using DataDomainEntities;

namespace DataAccessLayer
{
    

    public class PlayListMemoryRepository : IPlayListRepository
    {
        private readonly IDictionary<int, PlayList> _repo = new Dictionary<int, PlayList>();
        private int _cid = 0;

        public IEnumerable<PlayList> GetAll()
        {
            return _repo.Values;
        }

        public PlayList GetById(int id)
        {
            PlayList td = null;
            _repo.TryGetValue(id, out td);
            return td;
        }

        public void Add(PlayList td)
        {
            td.Id = _cid;
            _repo.Add(_cid++,td);
        }

        public void Remove(PlayList td)
        {  
            _repo.Remove(td.Id);
        }

        public bool VerifyUniqueName(string name)
        {
            foreach (PlayList b in this.GetAll())
            {
                if (b.Name.Equals(name))
                    return false;
            }

            return true;
        }

        public void SetUserPermition(int playListId, string nickname, Permition perm)
        {
            PlayList b = null;
            _repo.TryGetValue(playListId, out b);
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

        public PlayList GetPlayListByName(string name) 
        {
            //IEnumerable<Board> boards = GetAll();
            PlayList res = new PlayList();
            foreach (PlayList b in this.GetAll()) 
            {
                if (b.Name.Equals(name))
                    res = b;
            }

            return res;
        }
        public bool GetPlayListByNameAndUser(User user, string name)
        {
            //IEnumerable<Board> boards = GetAll();
           
            foreach (PlayList b in this.GetAll())
            {
                if (b.Name.Equals(name) && b.Owner.Equals(user.Nickname))
                    return false;
            }

            return true;
        }
    }
}