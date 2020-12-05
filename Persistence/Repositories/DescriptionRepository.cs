using Application.Interfaces.IRepositories;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories
{
    public class DescriptionRepository : GenericRepository<Description>, IDescriptionRepository
    {
        public DescriptionRepository(ApplicationContext context) : base(context)
        {
        }
    }
}