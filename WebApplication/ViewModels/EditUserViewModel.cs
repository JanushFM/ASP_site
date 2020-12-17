using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Roles = new List<string>();
        }

        public string Id { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Address { get; set; }
        
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public IList<string> Roles { get; set; }
    }
}