using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories
{
    public class AppUserRepository : GenericRepository<AppUser>, IAppUserRepository
    {
        // todo а надо ли тебе это ?
        private readonly ApplicationContext _context;

        public AppUserRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        // todo 
        // public override async Task<AppUser> GetById(int id)
        // {
        //     await _context.Set<Order>().Include(e => e.Painting)
        //         .Where(e => e.CustomerId == id).LoadAsync();
        //     
        //     var user = await _context.Set<AppUser>().FirstOrDefaultAsync(e => e.Id == id);
        //     return user;
        // }
    }
}