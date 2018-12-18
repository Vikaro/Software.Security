using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NetCoreWebsite.Data.Models;
using static NetCoreWebsite.Models.LoginSecondViewModel;

namespace NetCoreWebsite.Manager
{
    public interface IUserManager
    {
        Task<bool> SignIn(HttpContext httpContext, User user, bool isPersistent = false);
        Task SignInUserAsync(HttpContext httpContext, int userId);

        void SignOut(HttpContext httpContext);
        Task<SingInModel> FirstStepSignIn(User user);
        Task<SingInModel> SecondStepSignIn(int userId, string username, IDictionary<int, MaskViewModel> password);
        Task<int> GenerateSecondPasswordsAsync(int userId, string password);
        string GenerateRandomMask();
        int MaxLoginTries();
    }
}