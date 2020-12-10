using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.IRepositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        public Task<int> LoadOrdersWithArtistId(string userId);
    }
    
}