using Microsoft.AspNetCore.Identity;

namespace Domain.Seed
{
    public class SeedRoles
    {
        public static readonly string AdminRoleName = "Admin";
        public static readonly string UserRoleName = "User";
        public static readonly string SailorRoleName = "Sailor";
        
        
        public static void InitRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync(UserRoleName).Result)
            {
                var role = new IdentityRole();
                role.Name = UserRoleName;
                roleManager.CreateAsync(role);
            }


            if (!roleManager.RoleExistsAsync
                (AdminRoleName).Result)
            {
                IdentityRole role = new IdentityRole {Name = AdminRoleName};
                roleManager.CreateAsync(role);
            }
            
            if (!roleManager.RoleExistsAsync
                (SailorRoleName).Result)
            {
                IdentityRole role = new IdentityRole {Name = SailorRoleName};
                roleManager.CreateAsync(role);
            }
        }
    }
}