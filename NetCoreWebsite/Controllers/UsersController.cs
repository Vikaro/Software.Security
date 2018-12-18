using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetCoreWebsite.Data;
using NetCoreWebsite.Data.Models;
using NetCoreWebsite.Manager;
using NetCoreWebsite.Models;

namespace NetCoreWebsite.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserManager _userManager;
        private readonly string _SessionUserId = "sUserId";

        public UsersController(ApplicationDbContext context, IUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.Include(i => i.LoginLogs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [AllowAnonymous]
        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,PasswordHash")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        public async Task<IActionResult> EditPOST(int id, [Bind("Id,UserName,PasswordHash,MaxFailedCount, Locked")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> LoginPOST(User user)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(user.UserName)) return View("Login");
            //var signInModel = (await _userManager.FirstStepSignIn(user));
            //switch (signInModel.Status)
            //{
            //    case SignInStatus.Success:
            //        this.HttpContext.Session.SetString(this._SessionUserId, user.Id.ToString());
            //        return RedirectToAction("LoginSecond", new { userId = user.Id });
            //    case SignInStatus.Delayed:
            //        ModelState.AddModelError("Error", $" Your account is locked. Try again in {signInModel.Delayed}"); break;
            //    case SignInStatus.Failed:
            //        ModelState.AddModelError("Error", $" Wrong login or password. You will be locked until {signInModel.Delayed}.");
            //        //ModelState.AddModelError("Error", $" Your account is locked. Try again in {signInModel.Delayed}"); break;
            //        break;
            //    case SignInStatus.Locked:
            //        ModelState.AddModelError("Error", " Account is locked. Please contact administrator."); break;

            //}
            return RedirectToAction("LoginSecond", new { username = user.UserName });

            //return View("Login");


            //if ((await _userManager.FirstStepSignIn(user)).Status == SignInStatus.Success)
            //{

            //}
            //else
            //{

            //    if (delayDateTime > DateTime.Now)
            //    {

            //    }
            //    else if (user.Locked) ModelState.AddModelError("Error", " Account is locked");
            //    //else if (user.LockedUntil > DateTime.MinValue) ModelState.AddModelError("Error", $" Account is locked until {user.LockedUntil.ToString()}");
            //    else
            //    {
            //        ModelState.AddModelError("Error", " Wrong login or password");
            //        HttpContext.Session.SetInt32("FailedCount", ++failedCount);
            //        if (failedCount >= 3)
            //        {
            //            delay = DateTime.Now.AddSeconds(5 * (failedCount - 3)).ToString();
            //            HttpContext.Session.SetString("Delay", delay);
            //            ModelState.AddModelError("Error", $" Your account is locked. Try again in {delay}");
            //        }
            //    }

            //}
        }

        [AllowAnonymous]
        public async Task<IActionResult> LoginSecond(string username)
        {
            //var sUserId = this.HttpContext.Session.GetString(this._SessionUserId);
            //if (!(int.TryParse(sUserId, out int sessionUserId) && sessionUserId.Equals(userId)))
            //{
            //    ModelState.AddModelError("Error", " Session have expired");
            //    return View("Login");
            //}

            var model = new LoginSecondViewModel();
            var user = await this._context.Users.Include(i => i.SecondPassword).FirstOrDefaultAsync(i => i.UserName.Equals(username));
            if (user == null)
            {
                model.UserId = 0;
                model.Username = username;
                model.PasswordMask = this.GetMaskModel(username);
                //ModelState.AddModelError("Error", " User not found");
                //return View("Login");
            }
            else
            {
                //model.UserId = user.Id;
                model.Username = username;
                model.PasswordMask = this.GetMaskModel(user);
            }

            return View(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> LoginSecondPost(LoginSecondViewModel model)
        {
            if (!ModelState.IsValid) return View("Login");
            //var sUserId = this.HttpContext.Session.GetString(this._SessionUserId);
            //this.HttpContext.Session.Remove(this._SessionUserId);
            //if (!(int.TryParse(sUserId, out int sessionUserId) && sessionUserId.Equals(model.UserId)))
            //{
            //    ModelState.AddModelError("Error", " Session have expired");
            //    return View("Login");
            //}
            var signInModel = await this._userManager.SecondStepSignIn(model.UserId, model.Username,  model.PasswordMask);
            switch (signInModel.Status)
            {
                case SignInStatus.Success:
                    await this._userManager.SignInUserAsync(this.HttpContext, signInModel.UserId);
                    return RedirectToAction("Index", "Messages");
                case SignInStatus.Delayed:
                    ModelState.AddModelError("Error", $" Your account is locked. Try again in {signInModel.Delayed}"); break;
                case SignInStatus.Failed:
                    ModelState.AddModelError("Error", $" Wrong login or password. You will be locked until {signInModel.Delayed}.");
                    //ModelState.AddModelError("Error", $" Your account is locked. Try again in {signInModel.Delayed}"); break;
                    break;
                case SignInStatus.Locked:
                    ModelState.AddModelError("Error", " Account is locked. Please contact administrator."); break;
                default:
                    ModelState.AddModelError("Error", " Wrong login or password"); break;
                    //return View("Login");
            }
            return View("Login");

        }
        private Dictionary<int, LoginSecondViewModel.MaskViewModel> GetMaskModel(string username)
        {
            var dict = new Dictionary<int, LoginSecondViewModel.MaskViewModel>();

            var user = this._context.NotFoundUsers.FirstOrDefault(i => i.Username == username);
            if (user != null)
            {
                var passwordMask = user;

                for (int i = 0; i < passwordMask.Mask.Length; i++)
                {
                    var mask = new LoginSecondViewModel.MaskViewModel();
                    if (passwordMask.Mask[i] == '1')
                    {
                        mask.Mask = true;
                    }
                    dict.Add(i, mask);
                }
                return dict;
            }
            else
            {
                var randomMask = _userManager.GenerateRandomMask();
                var notFoundUser = new NotFoundUser()
                {
                    MaxFailedCount = _userManager.MaxLoginTries(),
                    Username = username,
                    Mask = _userManager.GenerateRandomMask()
                };
                this._context.NotFoundUsers.Add(notFoundUser);
                this._context.SaveChanges();
                var passwordMask = notFoundUser;

                for (int i = 0; i < passwordMask.Mask.Length; i++)
                {
                    var mask = new LoginSecondViewModel.MaskViewModel();
                    if (passwordMask.Mask[i] == '1')
                    {
                        mask.Mask = true;
                    }
                    dict.Add(i, mask);
                }
                return dict;
            }
        }
        private Dictionary<int, LoginSecondViewModel.MaskViewModel> GetMaskModel(User user)
        {
            var dict = new Dictionary<int, LoginSecondViewModel.MaskViewModel>();
            var passwordMask = user.SecondPassword.FirstOrDefault(i => i.Id == user.SecondPasswordId);

            for (int i = 0; i < passwordMask.Mask.Length; i++)
            {
                var mask = new LoginSecondViewModel.MaskViewModel();
                if (passwordMask.Mask[i] == '1')
                {
                    mask.Mask = true;
                }
                dict.Add(i, mask);
            }
            return dict;
        }
        [Authorize]
        public IActionResult SecondPasswordChange()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> SecondPasswordChangePOST(SecondPasswordChangeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = this.GetUserId();
                await this._userManager.GenerateSecondPasswordsAsync(userId, model.Password);
                return RedirectToAction("Edit", new { id = userId });
            }
            return View("SecondPasswordChange", model);
        }

        [Authorize]
        public async Task<IActionResult> SecondPasswordGeneratePOST(int userId)
        {
            await this._userManager.GenerateSecondPasswordsAsync(userId, "alamakota");
            return RedirectToAction("Edit", new { id = userId });
        }

        public IActionResult LogoutPOST()
        {
            _userManager.SignOut(this.HttpContext);
            return RedirectToAction("Login");
        }


        #region Helpers
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        private int GetUserId()
        {
            int.TryParse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value, out int userId);
            return userId;
        }
        #endregion
    }
}
