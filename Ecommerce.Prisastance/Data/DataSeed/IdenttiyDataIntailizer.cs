using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Prisastance.Data.DataSeed
{
    public class IdenttiyDataIntailizer : IDataIntilizer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdenttiyDataIntailizer> _logger;
        private readonly UserManager<ApplicationUser> _userManager;


        public IdenttiyDataIntailizer(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager, ILogger<IdenttiyDataIntailizer> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
            _userManager = userManager;
        }

        //Register in Program Cs
        public async Task IntilizeAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));

                }

                if (!_userManager.Users.Any())
                {
                    var User01 = new ApplicationUser()
                    {
                        DisplayName = "Sara Alaa",
                        UserName = "SaraAlaa",
                        Email = "zaa3748@gmail.com",
                        PhoneNumber = "1007224360"

                    };
                    var User02 = new ApplicationUser()
                    {
                        DisplayName = "Sara Mohamed",
                        UserName = "SaraMohamed",
                        Email = "SaraMohamed@gmail.com",
                        PhoneNumber = "1007224366"
                    };

                    await _userManager.CreateAsync(User01, "P@ss0rd");
                    await _userManager.CreateAsync(User02, "P@ss0rd");

                    await _userManager.AddToRoleAsync(User01, "Admin");
                    await _userManager.AddToRoleAsync(User02, "SuperAdmin");



                }
            }
            catch (Exception ex)
            {

               _logger.LogError($"Error occurred while seeding identity data: {ex.Message}");
            }
        }
    }
}
