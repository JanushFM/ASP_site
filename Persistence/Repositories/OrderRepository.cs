using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly ApplicationContext _context;

        public OrderRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }


        public override async Task<IEnumerable<Order>> GetAll()
        {
            return await _context.Set<Order>().Include(nameof(Painting)).ToListAsync();
        }

        public async Task<int> LoadOrdersWithArtistId(string userId)
        {
            var orders = await _context.Set<Order>().Include(e => e.Painting)
                .Where(e => e.AppUserId == userId).ToListAsync();

            return orders.Sum(order => order.Painting.Price * order.Amount);
        }

        public async Task<Order> GetById(string userId, int orderId)
        {
            return await _context.Set<Order>().Include(e => e.Painting)
                .FirstOrDefaultAsync(e => e.AppUserId == userId && e.Id == orderId);
        }

        public async Task<int> IsPaintingInOrder(string userId, int paintingId)
        {
            var orders = await GetOrdersWithUserId(userId);

            return (from order in orders where order.PaintingId == paintingId select order.Id).FirstOrDefault();
        }

        public async Task ConfirmOrders(IEnumerable<Order> ordersToConfirm)
        {
            foreach (var order in ordersToConfirm)
            {
                order.IsConfirmedByUser = true;
                await Update(order);
            }
        }

        public async Task<bool> IsPhoneNumberAssignedInOrders(string userId)
        {
            var orders = await GetOrdersWithUserId(userId);
            return orders.All(order => order.PhoneNumber != null && !order.PhoneNumber.Equals(""));
        }

        public async Task<List<Order>> GetOrdersWithUserId(string userId)
        {
            return await _context.Set<Order>()
                .Where(e => e.AppUserId == userId).ToListAsync(); // returns List<Order> with count 0
        }
    }
}