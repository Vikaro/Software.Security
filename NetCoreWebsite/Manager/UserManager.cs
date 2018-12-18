using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetCoreWebsite.Data;
using NetCoreWebsite.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static NetCoreWebsite.Models.LoginSecondViewModel;

namespace NetCoreWebsite.Manager
{
    public class SingInModel
    {
        public SignInStatus Status { get; set; }
        public DateTime Delayed { get; set; }
        public int FailedCount { get; set; }
        public int UserId { get; set; }
    }
    public enum SignInStatus
    {
        Success,
        Failed,
        Locked,
        Delayed
    }

    public enum SignInStep
    {
        FirstStep,
        SecondStep,
        ThirdStep
    }
    public class UserManager : IUserManager
    {
        private readonly Random random = new Random();
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly int MinimumMaskLength = 5;
        //private readonly int MaxLoginTries = 3;
        private readonly TimeSpan LoginDelay = new TimeSpan(0, 0, 5);

        public int MaxLoginTries()
        {
            return random.Next(0, 4);
        }
        private static Random Random = new Random();
        public UserManager(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        private int GetFailedCount(string username)
        {
            var userLogs = this._context.UserLogs.Where(i => i.UserName.Equals(username));
            var lastSuccesfull = userLogs.Where(i => i.Successfull == true).LastOrDefault();
            return lastSuccesfull == null ? userLogs.Count() : userLogs.Where(i => i.Date > lastSuccesfull.Date).Count();
        }

        private DateTime GetEnabledLoginTime(string userName, int count)
        {
            var userLogs = this._context.UserLogs.Where(i => i.UserName.Equals(userName));
            var lastFailed = userLogs.Where(i => i.Successfull == false).LastOrDefault();
            return (lastFailed == null) ? DateTime.Now : lastFailed.Date.Add(LoginDelay.Multiply(count));
        }

        private SingInModel IsUserLocked(User userDb, int failedCount, string username)
        {
            var notFoundUser = this._context.NotFoundUsers.FirstOrDefault(i => i.Username == username);
            if (userDb == null && notFoundUser == null)
            {
                notFoundUser = new NotFoundUser()
                {
                    MaxFailedCount = this.MaxLoginTries(),
                    Username = username,
                    Mask = GeneratePasswordMask(random.Next(8, 20))
                };
                this._context.NotFoundUsers.Add(notFoundUser);
                this._context.SaveChanges();
            }
            if ((userDb != null && (userDb.MaxFailedCount <= failedCount) && userDb.MaxFailedCount != 0) || (notFoundUser != null && ((notFoundUser.MaxFailedCount <= failedCount) && notFoundUser.MaxFailedCount != 0)))
            {
                return new SingInModel
                {
                    Status = SignInStatus.Locked
                };
            }
            return null;
        }

        public async Task<SingInModel> FirstStepSignIn(User inputUser)
        {
            var failedCount = this.GetFailedCount(inputUser.UserName);
            var userDb = this._context.Users.Include(i => i.LoginLogs).FirstOrDefault(i => i.UserName == inputUser.UserName);
            var result = new SingInModel();

            var isLocked = this.IsUserLocked(userDb, failedCount, inputUser.UserName);
            if (isLocked != null) return isLocked;
            var enabledLoginTime = this.GetEnabledLoginTime(inputUser.UserName, failedCount);
            if (DateTime.Now < enabledLoginTime)
            {
                return new SingInModel()
                {
                    Delayed = enabledLoginTime,
                    Status = SignInStatus.Delayed
                };
            }

            if (userDb != null && userDb.PasswordHash.Equals(inputUser.PasswordHash))
            {
                result.Status = SignInStatus.Success;
                inputUser.Id = userDb.Id;
            }
            else
            {
                result.Status = SignInStatus.Failed;
                result.Delayed = DateTime.Now.Add(LoginDelay.Multiply(failedCount + 1));
                result.FailedCount = failedCount;
            }
            this._context.UserLogs.Add(new UserLogs()
            {
                Date = DateTime.Now,
                Successfull = result.Status == SignInStatus.Success,
                User = userDb,
                UserName = inputUser.UserName,
                Step = SignInStep.FirstStep
            });

            await this._context.SaveChangesAsync();
            return result;

            //var dbUser = this._context.Users.Include(i => i.LoginLogs).Where(i => i.UserName.Equals(inputUser.UserName)).FirstOrDefault();

            ////var dbUser = this._context.Users.Include(i => i.LoginLogs).Where(i => i.UserName.Equals(user.UserName) && i.PasswordHash.Equals(user.PasswordHash)).FirstOrDefault();
            //if (dbUser != null)
            //{
            //    inputUser.Id = dbUser.Id;
            //    if (dbUser.Locked == true)
            //    {
            //        inputUser.Locked = dbUser.Locked;
            //        return false;
            //    }
            //    else
            //    {
            //        return true;
            //    }
            //}
            //else
            //{
            //    dbUser = this._context.Users.Include(i => i.LoginLogs).Where(i => i.UserName.Equals(inputUser.UserName)).FirstOrDefault();
            //    await this._context.UserLogs.AddAsync(new UserLogs
            //    {
            //        User = dbUser,
            //        Date = DateTime.Now,
            //        Successfull = false
            //    });
            //    if (dbUser != null)
            //    {
            //        var isMaxFailed = dbUser.LoginLogs.Where(i => i.Date >= dbUser.LastSuccesfullLogin && i.Successfull == false).Count() > dbUser.MaxFailedCount && dbUser.MaxFailedCount != 0;
            //        dbUser.Locked = isMaxFailed;
            //        inputUser.Locked = isMaxFailed;
            //        this._context.Update(dbUser);
            //    }
            //}
        }

        public async Task<SingInModel> SecondStepSignIn(int userId, string username, IDictionary<int, MaskViewModel> password)
        {
            var dbUser = await this._context.Users
                .Include(i => i.LoginLogs)
                .Include(i => i.SecondPassword)
                .Where(i => i.UserName.Equals(username))
                .FirstOrDefaultAsync();

            var failedCount = this.GetFailedCount(username);
            var isLocked = this.IsUserLocked(dbUser, failedCount, username);
            if (isLocked != null) return isLocked;
            var enabledLoginTime = this.GetEnabledLoginTime(username, failedCount);
            if (DateTime.Now < enabledLoginTime)
            {
                return new SingInModel()
                {
                    Delayed = enabledLoginTime,
                    Status = SignInStatus.Delayed
                };
            }
            var result = new SingInModel();
            if (dbUser != null)
            {
                var isSuccessful = CheckMaskedPassword(dbUser, dbUser.PasswordMask(), password, dbUser.SecondPasswordHash());

                //dbUser.LastSuccesfullLogin = dbUser.LoginLogs.Last()?.Date ?? DateTime.Now;
                dbUser.LastSuccesfullLogin = dbUser.LoginLogs.LastOrDefault(i => i.Successfull == true && i.Step == SignInStep.SecondStep)?.Date ?? DateTime.Now;
                if (isSuccessful)
                {
                    result.Status = SignInStatus.Success;
                    result.UserId = dbUser.Id;
                }
                else
                {
                    result.Status = SignInStatus.Failed;
                    result.Delayed = DateTime.Now.Add(LoginDelay.Multiply(failedCount + 1));
                    result.FailedCount = failedCount;
                }
            }
            else
            {
                result.Status = SignInStatus.Failed;
                result.Delayed = DateTime.Now.Add(LoginDelay.Multiply(failedCount + 1));
                result.FailedCount = failedCount;
            }
            this._context.UserLogs.Add(new UserLogs()
            {
                Date = DateTime.Now,
                Successfull = result.Status == SignInStatus.Success,
                User = dbUser,
                UserName = username,
                Step = SignInStep.FirstStep
            });

            await this._context.SaveChangesAsync();
            return result;


            //var userLog = new UserLogs()
            //{
            //    Date = DateTime.Now,
            //    Successfull = isSuccessful,
            //    User = dbUser,
            //    UserName = dbUser.UserName,
            //    Step = SignInStep.SecondStep
            //};

            //this._context.Update(dbUser);
            //this._context.UserLogs.Add(userLog);
            //await this._context.SaveChangesAsync();

            //return new SingInModel()
            //{
            //    Status = isSuccessful ? SignInStatus.Success : SignInStatus.Failed,
            //    Delayed = DateTime.Now.Add(LoginDelay.Multiply(failedCount + 1))
            //};
        }

        public async Task SignInUserAsync(HttpContext httpContext, int userId)
        {
            var dbUser = await this._context.Users.Include(i => i.SecondPassword).Include(i => i.LoginLogs).Where(i => i.Id.Equals(userId)).FirstOrDefaultAsync();
            if (dbUser != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(dbUser), CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                var secondPasswords = dbUser.SecondPassword.Where(i => i.Removed == false).ToList();
                var random = Random.Next(secondPasswords.Count());
                dbUser.SecondPasswordId = secondPasswords[random].Id;

                this._context.Update(dbUser);
                await this._context.SaveChangesAsync();
            }
        }

        private bool CheckMaskedPassword(User user, string mask, IDictionary<int, MaskViewModel> passwordDict, string hashedPassword)
        {
            string password = "";
            for (int i = 0; i < mask.Length; i++)
            {
                if (mask[i] == '1')
                {
                    if (passwordDict.TryGetValue(i, out var maskViewModel))
                    {
                        if (maskViewModel.Char != null)
                        {
                            password += maskViewModel.Char.First();
                        }
                    }
                    else
                    {
                        password += "0";
                    }
                }
                else
                {
                    password += "0";
                }
            }
            return this._passwordHasher.VerifyHashedPassword(user, hashedPassword, password).HasFlag(PasswordVerificationResult.Success);
        }
        private int MaskLength(string mask) => mask.Where(i => i == '1').Count();

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
                //dbUser.PasswordMask = this.GeneratePasswordMask(dbUser.PasswordHash.Count());

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

        public string GenerateRandomMask()
        {
            return this.GeneratePasswordMask(this.random.Next(8, 20));
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
            return string.Join("", mask.Select(i => Convert.ToInt32(i)));
        }
        private string GenerateMaskedPassword(string mask, string password)
        {
            string newPassword = "";
            for (int i = 0; i < mask.Length; i++)
            {
                if (mask[i] == '0')
                    newPassword += "0";
                else
                    newPassword += password[i];
            }
            return newPassword;
        }
        public async Task<int> GenerateSecondPasswordsAsync(int userId, string password)
        {
            int secondPasswordsCount = 10;
            var user = await this._context.Users
                .Include(i => i.LoginLogs)
                .Include(i => i.SecondPassword)
                .FirstOrDefaultAsync(i => i.Id == userId);
            foreach (var secondPassword in user.SecondPassword)
            {
                secondPassword.Removed = true;
                this._context.Update(secondPassword);
            }
            for (int i = 0; i < secondPasswordsCount; i++)
            {
                var secondPassword = new UserSecondPassword();
                var mask = this.GeneratePasswordMask(password.Length);
                var maskedPassword = this.GenerateMaskedPassword(mask, password);
                secondPassword.Mask = mask;
                secondPassword.User = user;
                secondPassword.Hash = _passwordHasher.HashPassword(user, maskedPassword);
                this._context.Add(secondPassword);
            }
            this._context.SaveChanges();

            var newSecondPassword = user.SecondPassword.Where(i => i.Removed != true).FirstOrDefault();
            user.SecondPasswordId = newSecondPassword.Id;
            this._context.Update(user);
            return this._context.SaveChanges();
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
