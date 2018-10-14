using Software.Security.Database.Services;
using Software.Security.Models.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Software.Security.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageRepository _messageRepository;
        private UserViewModel _user;
        public MessageController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;

        }
        public ActionResult AddMessage(string text)
        {
            if (GetCurrentUser() != null)
            {
                var model = this._messageRepository.AddMessage(text, 1);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            throw new UnauthorizedAccessException();
        }
        public ActionResult RemoveMessage(int id)
        {
            if (GetCurrentUser() != null)
            {
                var model = this._messageRepository.RemoveMessage(id);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            throw new UnauthorizedAccessException();

        }
        public ActionResult EditMessage(int id, string text)
        {
            if (GetCurrentUser() != null)
            {
                var model = this.EditMessage(id, text);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            throw new UnauthorizedAccessException();

        }

        private UserViewModel GetCurrentUser()
        {
            return _user = Session["CurrentUser"] as UserViewModel;
        }

    }

}