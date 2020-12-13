using System;
using System.IO;
using System.Net.Mime;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Maps
{
    public class PaintingMap : IEntityBuilder<Painting>
    {
        public PaintingMap(EntityTypeBuilder<Painting> builder) => BuildEntity(builder);

        public void BuildEntity(EntityTypeBuilder<Painting> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.ImageName).IsRequired();
            builder.Property(e => e.Price).IsRequired();
            builder.Property(e => e.NumberAvailable).IsRequired();
            builder.HasData
            (
                new Painting
                {
                    Id = 1,
                    Name = "Mona Lisa",
                    ImageName = "mono_lisa.jpg",
                    Price = 1465,
                    NumberAvailable = 10,
                    ArtistId = 2,
                    DescriptionId = 3
                },
                new Painting
                {
                    Id = 2,
                    Name = "Starry Night",
                    ImageName = "Starry_Night.jpg",
                    Price = 4656,
                    NumberAvailable = 6,
                    ArtistId = 1,
                    DescriptionId = 4
                }
            );
        }
    }
}