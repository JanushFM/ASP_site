using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string Address { get; set; }

        public List<Order> Orders { get; set; }
    }
}