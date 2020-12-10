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

        public override async Task<Order> GetById(int id)
        {
            return await _context.Set<Order>().Include(e => e.Painting)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<int> LoadOrdersWithArtistId(string userId)
        {
            var orders = await _context.Set<Order>().Include(e => e.Painting)
                .Where(e => e.AppUserId == userId).ToListAsync();

            return orders.Sum(order => order.Painting.Price);
        }

        public async Task<int> IsPaintingInOrder(string userId, int paintingId)
        {
            var orders = await _context.Set<Order>()
                .Where(e => e.AppUserId == userId).ToListAsync();

            if (orders.Any(order => order.PaintingId == paintingId))
            {
                return paintingId;
            }

            return 0;
        }
    }

}