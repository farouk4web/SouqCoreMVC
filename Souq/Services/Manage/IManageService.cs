using Souq.Models;
using Souq.Settings;
using Souq.ViewModels.Account;

namespace Souq.Services.Manage
{
    public interface IManageService
    {
        int GetNumberOfUsers();

        Task<IEnumerable<ApplicationUser>> GetAllUsers(int page);

        IEnumerable<ApplicationUser> GetLastSignedUsers(int number);



        bool IsExistUser(string userId);

        Task<ApplicationUser> GetUserById(string userId);



        Task<bool> IsExistRole(string roleName);

        Task<IEnumerable<string>> GetUserRoles(string userId);

        Task<IEnumerable<ApplicationUser>> GetUsersInRole(string role);



        Task<bool> CheckPremetionAsync(string currentUserId, string role);

        Task<ServiceResult> AddToRoleAsync(RoleRequest model);

        Task<ServiceResult> RemoveFromRoleAsync(RoleRequest model);
    }
}