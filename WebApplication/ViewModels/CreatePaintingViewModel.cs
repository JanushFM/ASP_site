using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace WebApplication.ViewModels
{
    public class CreatePaintingViewModel
    {
        public virtual int Id { get; set; }
        
        [StringLength(120, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Please choose  image")]  
        [Display(Name = "Picture")]
        public IFormFile Image { get; set; }
        
        public Description Description { get; set; }
        
        [Required]
        public int Price { get; set; }

        public int NumberAvailable { get; set; }
        
        public IEnumerable<Artist> Artists { get; set; }
        
        public int SelectedArtistId { get; set; }

    }
}