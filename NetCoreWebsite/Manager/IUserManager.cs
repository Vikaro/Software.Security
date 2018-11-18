using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NetCoreWebsite.Data.Models;

namespace NetCoreWebsite.Manager
{
    public interface IUserManager
    {
        Task<bool> SignIn(HttpContext httpContext, User user, bool isPersistent = false);
        void SignOut(HttpContext httpContext);
        Task<bool> FirstStepSignIn(User user);
        Task<bool> SecondStepSignIn(HttpContext httpContext, string userName, string password);
    }
}