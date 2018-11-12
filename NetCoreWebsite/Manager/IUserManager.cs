using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NetCoreWebsite.Data.Models;

namespace NetCoreWebsite.Manager
{
    public interface IUserManager
    {
        Task<bool> SignIn(HttpContext httpContext, User user, bool isPersistent = false);
        void SignOut(HttpContext httpContext);
    }
}