using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDomainEntities
{
    public class PlayListManager
    {
        public List<PlayList> playLists = new List<PlayList>();


        public List<PlayList> getAll()
        {
            return playLists;
        }

        public PlayList getPlayListById(int id) {
            foreach (PlayList b in playLists) {
                if (b.Id == id) {
                    return b;
                }
            }
            return null;
        }
    }
}
