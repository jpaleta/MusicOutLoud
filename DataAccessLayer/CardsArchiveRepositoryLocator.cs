using System.Collections.Generic;
using DataDomainEntities;

namespace DataAccessLayer
{
    public class CardsArchiveRepositoryLocator
    {
        private readonly static ICardsArchiveRepository Repo = new CardsArchiveMemoryRepository();
        public static ICardsArchiveRepository Get()
        {
            return Repo;
        }
    }
}
