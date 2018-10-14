using Software.Security.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Software.Security.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthorizationRepository _authorizationService;

        public HomeController(IAuthorizationRepository authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public ActionResult Index()
        {
            return View();
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
            this._authorizationService.Login(login, password);
            return View();
        }
    }
}