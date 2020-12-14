using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebApplication
{
    public class DataInitializer
    {
        private static readonly string AdminRoleName = "Admin";
        private static readonly string UserRoleName = "User";
        private static readonly string SailorRoleName = "Sailor";
        
         public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager) {
            
            if (await roleManager.FindByNameAsync(AdminRoleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole {Name = AdminRoleName});
            }
            if (await roleManager.FindByNameAsync(UserRoleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole {Name = UserRoleName});
            }
            if (await roleManager.FindByNameAsync(SailorRoleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole {Name = SailorRoleName});
            }
        }
    }
}