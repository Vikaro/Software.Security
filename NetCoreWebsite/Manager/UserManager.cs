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
        private readonly int MinimumMaskLength = 5;
        private static Random Random = new Random();
        public UserManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> FirstStepSignIn(User user)
        {
            var dbUser = this._context.Users.Include(i => i.LoginLogs).Where(i => i.UserName.Equals(user.UserName) && i.PasswordHash.Equals(user.PasswordHash)).FirstOrDefault();
            if (dbUser != null)
            {
                user.Id = dbUser.Id;
                if (dbUser.Locked == true)
                {
                    user.Locked = dbUser.Locked;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                dbUser = this._context.Users.Include(i => i.LoginLogs).Where(i => i.UserName.Equals(user.UserName)).FirstOrDefault();
                await this._context.UserLogs.AddAsync(new UserLogs
                {
                    User = dbUser,
                    Date = DateTime.Now,
                    Successfull = false
                });
            }
            await this._context.SaveChangesAsync();
            return false;
        }

        public async Task<bool> SecondStepSignIn(HttpContext httpContext, string userName, string password)
        {
            var dbUser = await this._context.Users.Include(i => i.LoginLogs).Where(i => i.UserName.Equals(userName)).FirstOrDefaultAsync();
            if (dbUser != null)
            {
                var mask = dbUser.PasswordMask;
                var isValid = CheckMaskedPassword(dbUser.PasswordMask, password, dbUser.SecondPassword);
            }
            return true;
        }

        public async void SignInUser(HttpContext httpContext, User user)
        {
            ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(user), CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        private bool CheckMaskedPassword(string mask, string inputPassword, string userPassword)
        {
            var validSecondPasswordChars = new List<bool>();
            var maskLength = 0;
            for (int i = 0; i < mask.Length; i++)
            {
                if (mask[i].Equals('1'))
                {
                    ++maskLength;
                    validSecondPasswordChars.Add(inputPassword.Equals(userPassword[i]));
                }
            }
            return validSecondPasswordChars.Count.Equals(maskLength);
        }

        public async Task<bool> SignIn(HttpContext httpContext, User user, bool isPersistent = false)
        {
            bool result = false;

            var dbUser = this._context.Users.Include(i => i.LoginLogs).Where(i => i.UserName.Equals(user.UserName) && i.PasswordHash.Equals(user.PasswordHash)).FirstOrDefault();
            if (dbUser != null)
            {
                result = true;
                if (dbUser.Locked == true)
                {
                    user.Locked = dbUser.Locked;
                    return false;
                }

                ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(dbUser), CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                var lastSuccesfull = dbUser.LoginLogs.Where(i => i.Successfull == true).LastOrDefault();
                dbUser.LastSuccesfullLogin = lastSuccesfull?.Date ?? DateTime.Now;
                dbUser.PasswordMask = this.GeneratePasswordMask(dbUser.PasswordHash.Count());

                this._context.Update(dbUser);
                await this._context.UserLogs.AddAsync(new UserLogs
                {
                    User = dbUser,
                    Date = DateTime.Now,
                    Successfull = true
                });
                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            }
            else
            {
                dbUser = this._context.Users.Include(i => i.LoginLogs).Where(i => i.UserName.Equals(user.UserName)).FirstOrDefault();
                await this._context.UserLogs.AddAsync(new UserLogs
                {
                    User = dbUser,
                    Date = DateTime.Now,
                    Successfull = result
                });
                if (dbUser != null)
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

        private string GeneratePasswordMask(int passwordLength)
        {
            int maskLength = (passwordLength > 10) ? passwordLength / 2 : this.MinimumMaskLength;
            bool[] mask = new bool[passwordLength];

            List<int> passwordCharsPositions = new List<int>();
            for (int i = 0; i < passwordLength; i++) passwordCharsPositions.Add(i);
            for (int i = 0; i < maskLength; i++)
            {
                var index = Random.Next(passwordCharsPositions.Count);
                mask[passwordCharsPositions[index]] = true;
                passwordCharsPositions.RemoveAt(index);
            }
            return string.Join("", passwordCharsPositions);
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
