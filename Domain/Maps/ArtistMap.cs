using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Maps
{
    public class ArtistMap : IEntityBuilder<Artist>
    {
        public ArtistMap(EntityTypeBuilder<Artist> builder) => BuildEntity(builder);

        public void BuildEntity(EntityTypeBuilder<Artist> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired();
            // builder.Property(e => e.Description).IsRequired();
            builder.Property(e => e.ImageName).IsRequired();
            builder.Property(e => e.Quote).IsRequired();
            builder.HasData
            (
                new Artist
                {
                    Id = 1,
                    Name = "Vincent van Gogh",
                    ImageName = "Vincent_van_Gogh__portrait.jpg",
                    Quote = "quote",
                    DescriptionId = 1
                },
                new Artist
                {
                    Id = 2,
                    Name = "Leonardo da Vinci",
                    ImageName = "Leonardo_da_Vinci_portrait_Ti9m3nK.jpg",
                    Quote =
                        "Painting is poetry that is seen rather than felt," +
                        " and poetry is painting that is felt rather than seen",
                    DescriptionId = 2
                        
                }
            );
        }
    }
}