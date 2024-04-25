using Microsoft.AspNetCore.Identity;
using Souq.Data;
using Souq.Models;
using Souq.Settings;

namespace Souq.StartUp
{
    public static class SeedData
    {
        public static async Task SeedDefaultDataAsync(ApplicationDbContext _context, RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager)
        {
            try
            {
                _context.Database.EnsureCreated();


                // Seed Roles
                List<string> rolesNames = new() { RoleName.Owners, RoleName.Admins, RoleName.Sellers, RoleName.ShippingStaff };

                foreach (var role in rolesNames)
                    if (!await _roleManager.RoleExistsAsync(role))
                        await _roleManager.CreateAsync(new IdentityRole() { Name = role });


                // Developer Account 
                ApplicationUser developerAccount = new()
                {
                    UserName = "farouk",
                    Email = "farouk@souq.com",
                    EmailConfirmed = true,

                    FirstName = "Farouk",
                    LastName = "Abdelhamid",
                    ProfileImageSrc = "/uploads/users/user.png",
                    JoinDate = DateTime.UtcNow
                };

                // Seed Developer Account 
                if (!_userManager.Users.Any(u => u.Email == "farouk@souq.com"))
                {
                    // Create and Insert Developer Account Into Owner Role
                    await _userManager.CreateAsync(developerAccount, "Abc@123");
                    await _userManager.AddToRoleAsync(developerAccount, RoleName.Owners);


                    // create shooppingCart For Developer Account
                    Cart developerCart = new() { UserId = developerAccount.Id };
                    _context.ShopingCarts.Add(developerCart);
                    _context.SaveChanges();
                }


                if (!_context.PaymentMethods.Any())
                {
                    // PaymentMethods
                    List<PaymentMethod> methods = new()
                    {
                        new PaymentMethod(){Name = "Cash On Delivery",Fee= 10},
                        new PaymentMethod(){Name = "Paypal",Fee= 0},
                        new PaymentMethod(){Name = "VisaCard",Fee= 0}
                        //,
                        //new PaymentMethod(){Name = "PhoneWallet",Fee= 0}
                    };

                    _context.PaymentMethods.AddRange(methods);
                    _context.SaveChanges();
                }


            }
            catch (Exception)
            {
                throw;
            }


        }
    }
}