using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Description : BaseEntity
    {
        [StringLength(200)]
        [Required]
        [DefaultValue("No SMALL description provided")]
        public string SmallDescription { get; set; }
        
        [StringLength(400)]
        [Required]
        [DefaultValue("No BIG description provided")]
        public string BigDescription { get; set; }
    }
}