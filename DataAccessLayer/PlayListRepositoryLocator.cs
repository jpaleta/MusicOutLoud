using DataDomainEntities;

namespace DataAccessLayer
{
    

    public class PlayListRepositoryLocator
    {
        private readonly static IPlayListRepository Repo = new PlayListMemoryRepository();
        public static IPlayListRepository Get()
        {
            return Repo;
        }
    }
}