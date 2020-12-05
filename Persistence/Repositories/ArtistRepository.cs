using System.Collections.Generic;
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
    }
}