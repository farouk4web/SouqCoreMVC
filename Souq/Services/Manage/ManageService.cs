using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Souq.Models;
using Souq.Services.Files;
using Souq.Settings;
using Souq.ViewModels.Account;

namespace Souq.Services.Manage
{
    public class ManageService : IManageService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IFileService _fileService;

        public ManageService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IFileService fileService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _fileService = fileService;
        }

        public int GetNumberOfUsers()
        {
            var count = _userManager.Users.Count();
            return count;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers(int page)
        {
            var elementsPerPage = 10;
            var skippedElements = page * elementsPerPage - elementsPerPage;
            var users = await _userManager.Users
                                    .OrderByDescending(c => c.JoinDate)
                                    .Skip(skippedElements)
                                    .Take(elementsPerPage)
                                    .ToListAsync();
            return users;
        }

        public IEnumerable<ApplicationUser> GetLastSignedUsers(int number)
        {
            var lastSignedUsers = _userManager.Users.OrderByDescending(u => u.JoinDate).Take(number).ToList();

            return lastSignedUsers;
        }



        public bool IsExistUser(string userId)
        {
            var result = _userManager.Users.Any(u => u.Id == userId);
            return result;
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            var userInDb = await _userManager.FindByIdAsync(userId);
            return userInDb;
        }

        public async Task<IEnumerable<string>> GetUserRoles(string userId)
        {
            var userInDb = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(userInDb);
            return userRoles;
        }

        public async Task<bool> IsExistRole(string roleName)
        {
            var result = await _roleManager.RoleExistsAsync(roleName);
            return result;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersInRole(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);
            return users;
        }




        // Manage User Roles
        public async Task<bool> CheckPremetionAsync(string currentUserId, string role)
        {
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            if (!await _userManager.IsInRoleAsync(currentUser, RoleName.Owners) && role == RoleName.Owners)
                return false;

            return true;
        }

        public async Task<ServiceResult> AddToRoleAsync(RoleRequest model)
        {
            ServiceResult serviceResult = new();

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (await _userManager.IsInRoleAsync(user, model.Role))
            {
                serviceResult.Message = "User Is already assigned to this role";
                return serviceResult;
            }

            var result = await _userManager.AddToRoleAsync(user, model.Role);
            if (!result.Succeeded)
                serviceResult.Message = "Something went wrong, Please Try Again";
            else
                serviceResult.Message = $"{user.UserName} Is Added Successfully to {model.Role}.";

            return serviceResult;
        }

        public async Task<ServiceResult> RemoveFromRoleAsync(RoleRequest model)
        {
            ServiceResult serviceResult = new();

            var user = await _userManager.FindByIdAsync(model.UserId);

            var result = await _userManager.RemoveFromRoleAsync(user, model.Role);
            if (!result.Succeeded)
                serviceResult.Message = "Something went wrong, Please Try Again";
            else
                serviceResult.Message = $"{user.UserName} Is Removed Successfully From {model.Role}.";

            return serviceResult;
        }

        
    }
}
