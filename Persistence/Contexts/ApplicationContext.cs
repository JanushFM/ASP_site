using Domain.Entities;
using Domain.Maps;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new MovieMap(modelBuilder.Entity<Movie>());
            new DescriptionMap(modelBuilder.Entity<Description>());
            new ArtistMap(modelBuilder.Entity<Artist>());
            
            modelBuilder.Entity<Artist>()
                .HasOne(b => b.Description)
                .WithOne()
                .HasForeignKey<Description>(p => p.Id);
            
        }
    }
}