using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories
{
    public class ArtistRepository : GenericRepository<Artist>, IArtistRepository
    {
        private readonly ApplicationContext _context;

        public ArtistRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Artist>> GetAll()
        {
            return await _context.Set<Artist>().Include(nameof(Description)).ToListAsync();
        }

        public override async Task<Artist> GetById(int id)
        {
            await _context.Set<Painting>().Include(e => e.Description)
                .Where(e => e.ArtistId == id).LoadAsync();
            
            var artist = await _context.Set<Artist>().Include(e => e.Description)
                .FirstOrDefaultAsync(e => e.Id == id);
            
            return artist;
        }
    }
}