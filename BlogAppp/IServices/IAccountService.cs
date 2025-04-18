using BlogAppp.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BlogAppp.IServices
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);

        Task LogoutAsync();
    }
}
