using System.ComponentModel.DataAnnotations;

namespace WebApplication.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}