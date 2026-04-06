using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationAPP.Data
{
    public static class SeedRoles
    {
        public static async Task CrearRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Administrador"))
            {
                await roleManager.CreateAsync(new IdentityRole("Administrador"));
            }

            if (!await roleManager.RoleExistsAsync("Participante"))
            {
                await roleManager.CreateAsync(new IdentityRole("Participante"));
            }
        }
    }
}