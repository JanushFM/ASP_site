using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Artist : BaseEntity
    {
        [StringLength(120, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        public int DescriptionId { get; set; }
        
        public string ImageName { get; set; }

        public string Quote { get; set; }
    }
}