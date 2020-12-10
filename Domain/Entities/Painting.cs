using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Painting : BaseEntity
    {
        [StringLength(120, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        public string ImageName { get; set; }
        
        public int DescriptionId { get; set; }
        public Description Description { get; set; }

        public int ArtistId { get; set; }

        
        public int Price { get; set; }

        public int NumberAvailable { get; set; }
    }
}