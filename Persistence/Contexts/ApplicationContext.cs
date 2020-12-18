using Domain.Entities;
using Domain.Maps;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Persistence.Contexts
{
    public class ApplicationContext : IdentityDbContext<AppUser>
    {

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new DescriptionMap(modelBuilder.Entity<Description>());
            new ArtistMap(modelBuilder.Entity<Artist>());
            new PaintingMap(modelBuilder.Entity<Painting>());
           
            modelBuilder.Entity<Artist>()
                .HasOne(b => b.Description)
                .WithOne()
                .HasForeignKey<Artist>(p => p.DescriptionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Painting>()
                .HasOne(b => b.Description)
                .WithOne()
                .HasForeignKey<Painting>(p => p.DescriptionId)
                .OnDelete(DeleteBehavior.NoAction);;
            
            modelBuilder.Entity<AppUser>()
                .HasMany(b => b.Orders)
                .WithOne()
                .HasForeignKey(e => e.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}