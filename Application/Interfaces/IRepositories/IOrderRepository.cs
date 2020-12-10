using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.IRepositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        public Task<int> LoadOrdersWithArtistId(string userId);
        public Task<Order> GetById(string userId, int orderId);
        public Task<int> IsPaintingInOrder(string userId, int paintingId);
        public Task ConfirmOrders(string userId);
        public Task<bool> IsPhoneNumberAssignedInOrders(string userId);
    }
    
}