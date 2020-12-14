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
        
        public IFormFile Image { get; set; }

        public string PrevImageName { get; set; }

        public string Quote { get; set; }
    }
}