using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace WebApplication.ViewModels
{
    public class EditArtistViewModel
    {
        public int Id { get; set; }
        [StringLength(120, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }
        
        public Description Description { get; set; }

        [Required(ErrorMessage = "Please choose  image")]  
        [Display(Name = "Picture")]
        public IFormFile Image { get; set; }

        public string Quote { get; set; }
    }
}