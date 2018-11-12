using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreWebsite.Models;
using NetCoreWebsite.Repositories;

namespace NetCoreWebsite.Controllers
{
    public class HomeController : Controller
    {

        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IMessageRepository _messageRepository;
        //private readonly IMapper _mapper;
        public HomeController(IAuthorizationRepository authRepository, IMessageRepository messageRepository)
        {
            _authorizationRepository = authRepository;
            _messageRepository = messageRepository;
            //_mapper = mapper;
        }

        public IActionResult Index()
        {
            IEnumerable<Data.Models.Message> messages = this._messageRepository.GetMessages();

            return View(messages);
        }

        

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
