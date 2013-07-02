using System.Collections.Generic;
using DataDomainEntities;

namespace DataAccessLayer
{
    public class ListsRepositoryLocator
    {
        private readonly static IListsRepository Repo = new ListsMemoryRepository();
        public static IListsRepository Get()
        {
            return Repo;
        }
    }
}