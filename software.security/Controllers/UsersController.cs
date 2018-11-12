using AutoMapper;
using Software.Security.Attributes;
using Software.Security.Database.Repository;
using Software.Security.Models.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Software.Security.Controllers
{
    public class UsersController : Controller
    {
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        public UsersController(IAuthorizationRepository authRepository, IMessageRepository messageRepository, IMapper mapper)
        {
            _authorizationRepository = authRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
        }
        // GET: Users
        [HttpGet]
        [Route("{id}")]
        [CustomAuthorize]
        public ActionResult UserPanel(int id)
        {


            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult LoginPOST(string login, string password)
        {
            if (this._authorizationRepository.IsUserExist(login, password))
            {
                var user = this._authorizationRepository.GetUser(login);
                var model = this._mapper.Map<UserViewModel>(user);
                //Session.Add("CurrentUser", model);

                FormsAuthentication.SetAuthCookie(user.Name, false);
                this.ViewBag.Message = "Login succeed";
                return RedirectToAction("Index", "Home");
            }
            this.ViewBag.Message = "Login failed";
            return RedirectToAction("Login");
        }
    }
}