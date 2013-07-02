using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDomainEntities
{
    public class BoardManager
    {
        public List<Board> boards = new List<Board>();


        public List<Board> getAll()
        {
            return boards;
        }

        public Board getBoardById(int id) {
            foreach (Board b in boards) {
                if (b.Id == id) {
                    return b;
                }
            }
            return null;
        }
    }
}
