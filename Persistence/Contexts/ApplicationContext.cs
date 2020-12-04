using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() {}
        protected readonly string databaseName = "shop_db";
        public DbSet<Movie> Movie { get; set; }

    }
}