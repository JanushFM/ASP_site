using System.Linq;
using Domain.Entities;
using Domain.Maps;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Persistence.Contexts
{
    public class ApplicationContext : IdentityDbContext<AppUser>
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
            new PaintingMap(modelBuilder.Entity<Painting>());
            
            // После загрузки на сервер с подходящей СУБД проверь правильно ли работает удаление (удали картину, не удалиться ли автор)
            // Удали Заказ, не удалиться ли картина. А если удалиться картина удалиться ли заказ ?
            // Попробуй также удалить роль, или юсера (удалиться юсер, а роль ?)
            modelBuilder.Entity<Artist>()
                .HasOne(b => b.Description)
                .WithOne()
                .HasForeignKey<Artist>(p => p.DescriptionId);

            modelBuilder.Entity<Painting>()
                .HasOne(b => b.Description)
                .WithOne()
                .HasForeignKey<Painting>(p => p.DescriptionId);
            
            modelBuilder.Entity<Order>()
                .HasOne(b => b.Painting)
                .WithOne()
                .HasForeignKey<Order>(p => p.PaintingId);
            
            // foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            // {
            //     foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            // }
        }
    }
}