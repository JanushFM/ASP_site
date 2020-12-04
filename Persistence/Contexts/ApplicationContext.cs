using Domain.Entities;
using Domain.Maps;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() {}
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {}
        
        protected readonly string databaseName = "shop_db";
        public DbSet<Movie> Movie { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new MovieMap(modelBuilder.Entity<Movie>());
        }
        
    }
}