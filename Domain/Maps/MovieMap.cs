using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Maps
{
    public class MovieMap : IEntityBuilder<Movie>
    {
        public MovieMap(EntityTypeBuilder<Movie> builder) => BuildEntity(builder);
        
        public void BuildEntity(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Title).IsRequired();
            builder.Property(e => e.ReleaseDate).IsRequired();
            builder.Property(e => e.Genre).IsRequired();
            builder.Property(e => e.Price).IsRequired();
            builder.Property(e => e.Rating).IsRequired();
            builder.HasData
            (
                new Movie
                {
                    Id = 1,
                    Title = "When Harry Met Sally",
                    ReleaseDate = DateTime.Parse("1989-2-12"),
                    Genre = "Romantic Comedy",
                    Price = 7.99M,
                    Rating = "R"
                },
                new Movie
                {
                    Id = 2,
                    Title = "Ghostbusters ",
                    ReleaseDate = DateTime.Parse("1984-3-13"),
                    Genre = "Comedy",
                    Price = 8.99M,
                    Rating = "R"
                },
                new Movie
                {
                    Id = 3,
                    Title = "Ghostbusters 2",
                    ReleaseDate = DateTime.Parse("1986-2-23"),
                    Genre = "Comedy",
                    Price = 9.99M,
                    Rating = "R"
                },
                new Movie
                {
                    Id = 4,
                    Title = "Rio Bravo",
                    ReleaseDate = DateTime.Parse("1959-4-15"),
                    Genre = "Western",
                    Price = 3.99M,
                    Rating = "R"
                }
            );
        }
    }
}