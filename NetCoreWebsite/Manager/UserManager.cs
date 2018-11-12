using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using NetCoreWebsite.Data;
using NetCoreWebsite.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NetCoreWebsite.Manager
{
    public class UserManager : IUserManager
    {
        private readonly ApplicationDbContext _context;

        public UserManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SignIn(HttpContext httpContext, User inputUser, bool isPersistent = false)
        {
            var user = this._context.Users.Where(i => i.UserName.Equals(inputUser.UserName) && i.PasswordHash.Equals(inputUser.PasswordHash)).FirstOrDefault();
            if (user != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(user), CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return true;
            }
            return false;
        }

        public async void SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }

        private IEnumerable<Claim> GetUserClaims(User user)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            //claims.Add(new Claim(ClaimTypes.Email, user.UserEmail));
            //claims.AddRange(this.GetUserRoleClaims(user));
            return claims;
        }

        //private IEnumerable<Claim> GetUserRoleClaims(UserDbModel user)
        //{
        //    List<Claim> claims = new List<Claim>();

        //    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id().ToString()));
        //    claims.Add(new Claim(ClaimTypes.Role, user.UserPermissionType.ToString()));
        //    return claims;
        //}

    }
}
