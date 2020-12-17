using System.ComponentModel.DataAnnotations;

namespace WebApplication.ViewModels
{
    public class AccountSettingsViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Address { get; set; }
        
        [Phone] 
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}