﻿using meusite.Data;
using Microsoft.AspNetCore.Identity;

namespace MeuSite.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider
                .GetRequiredService<MeuSiteContext>();

            context.Database.EnsureCreated();

            var roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            var roleName = "Administrator";

            IdentityResult result;

            var roleExist = await roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                result = await roleManager
                    .CreateAsync(new IdentityRole(roleName));

                if (result.Succeeded)
                {
                    var userManager = serviceProvider
                        .GetRequiredService<UserManager<IdentityUser>>();

                    var config = serviceProvider
                        .GetRequiredService<IConfiguration>();

                    var admin = await userManager
                        .FindByEmailAsync(config["AdminCredentials:Email"]);
                    if (admin == null)
                    {
                        admin = new IdentityUser()
                        {
                            UserName = config["AdminCredentials:Email"],
                            Email = config["AdminCredentials:Email"],
                            EmailConfirmed = true
                        };
                        result = await userManager
                            .CreateAsync(admin, config["AdminCredentials:Password"]);

                        if (result.Succeeded)
                        {
                            result = await userManager
                                .AddToRoleAsync(admin, roleName);
                            if (!result.Succeeded)
                            {
                                // todo:processar erros
                            }
                        }
                    }
                }
            }
        }
    }
}
