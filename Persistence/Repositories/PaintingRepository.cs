using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories
{
    public class PaintingRepository : GenericRepository<Painting>, IPaintingRepository
    {
        private readonly ApplicationContext _context;

        public PaintingRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
        
        public override async Task<IEnumerable<Painting>> GetAll()
        {
            return await _context.Set<Painting>().Include(nameof(Description)).ToListAsync();
        }

        public override async Task<Painting> GetById(int id)
        {
            return await _context.Set<Painting>().Include(nameof(Description))
                .FirstOrDefaultAsync(artist => artist.Id == id);
        }
    }
}