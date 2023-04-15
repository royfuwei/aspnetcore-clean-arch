
using CleanArch.Application.Common.Models;

namespace CleanArch.Application.Modules.Identity.UseCases;

public interface IIdentityService
{
    Task<List<ApplicationUser>> GetUsersAsync();

    Task<string?> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(string userId);
}
