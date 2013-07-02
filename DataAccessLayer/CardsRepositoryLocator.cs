using System.Collections.Generic;
using DataDomainEntities;

namespace DataAccessLayer
{
    public class CardsRepositoryLocator
    {
        private readonly static ICardsRepository Repo = new CardsMemoryRepository();
        public static ICardsRepository Get()
        {
            return Repo;
        }
    }
}
