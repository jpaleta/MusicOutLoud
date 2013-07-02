using DataDomainEntities;

namespace DataAccessLayer
{
    

    public class BoardsRepositoryLocator
    {
        private readonly static IBoardsRepository Repo = new BoardsMemoryRepository();
        public static IBoardsRepository Get()
        {
            return Repo;
        }
    }
}