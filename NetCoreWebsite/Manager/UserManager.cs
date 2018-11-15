using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
            bool result = false;

            var user = this._context.Users.Include(i=> i.LoginLogs) .Where(i => i.UserName.Equals(inputUser.UserName) && i.PasswordHash.Equals(inputUser.PasswordHash)).FirstOrDefault();
            if (user != null)
            {
                result = true;
                if (user.Locked == true || user.LockedUntil >= DateTime.Now)
                {
                    inputUser.LockedUntil = user.LockedUntil;
                    inputUser.Locked = user.Locked;
                    return false;
                }

                ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(user), CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                var lastSuccesfull = user.LoginLogs.Where(i => i.Successfull == true).LastOrDefault();
                user.LastSuccesfullLogin = lastSuccesfull?.Date ?? DateTime.Now;
                this._context.Update(user);
                await this._context.UserLogs.AddAsync(new UserLogs
                {
                    User = user,
                    Date = DateTime.Now,
                    Successfull = true
                });
                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            }
            else
            {
                user = this._context.Users.Include(i=> i.LoginLogs).Where(i => i.UserName.Equals(inputUser.UserName)).FirstOrDefault();
                await this._context.UserLogs.AddAsync(new UserLogs
                {
                    User = user,
                    Date = DateTime.Now,
                    Successfull = result
                });
                if (user != null)
                {
                    //var failedCount = user.LoginLogs.Where(i => i.Successfull == false && i.Date > user.LastSuccesfullLogin).Count();
                    //if (user.MaxFailedCount > 0 && failedCount >= user.MaxFailedCount)
                    //{
                    //    user.Locked = true;
                    //}
                    //else if(user.MaxFailedCount > 0)
                    //{
                    //    user.LockedUntil = DateTime.Now.AddSeconds(failedCount * 10);
                    //}
                }
            }
            await this._context.SaveChangesAsync();
            return result;
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
