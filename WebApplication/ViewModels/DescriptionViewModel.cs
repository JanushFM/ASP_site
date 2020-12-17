using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.ViewModels
{
    public class DescriptionViewModel
    {
        [StringLength(600)]
        [Required]
        [DefaultValue("No SMALL description provided")]
        public string SmallDescription { get; set; }
        
        [StringLength(4000)]
        [Required]
        [DefaultValue("No BIG description provided")]
        public string BigDescription { get; set; }
    }
}