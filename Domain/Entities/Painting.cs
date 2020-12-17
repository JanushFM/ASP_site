namespace Domain.Entities
{
    public class Painting : BaseEntity
    {
        public string Name { get; set; }

        public string ImageName { get; set; }
        public string ImageUri { get; set; }
        public string ThumbnailUri { get; set; }

        public int DescriptionId { get; set; }
        public Description Description { get; set; }

        public int ArtistId { get; set; }

        public int Price { get; set; }

        public int NumberAvailable { get; set; }
    }
}