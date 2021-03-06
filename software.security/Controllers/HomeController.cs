﻿using AutoMapper;
using Software.Security.Database;
using Software.Security.Database.Repository;
using Software.Security.Models.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Software.Security.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        public HomeController(IAuthorizationRepository authRepository, IMessageRepository messageRepository, IMapper mapper)
        {
            _authorizationRepository = authRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            IEnumerable<Database.Models.Message> messages = this._messageRepository.GetMessages();

            return View(messages);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult UserLogin(string login, string password)
        {
            if (this._authorizationRepository.IsUserExist(login, password))
            {
                var user = this._authorizationRepository.GetUser(login);
                var model = this._mapper.Map<UserViewModel>(user);
                Session.Add("CurrentUser", model);

                this.ViewBag.Message = "Login succeed";
                return RedirectToAction("Index");
            }
            this.ViewBag.Message = "Login failed";
            return RedirectToAction("Login");
        }
    }
}