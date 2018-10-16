using Software.Security.Database.Repository;
using Software.Security.Models;
using Software.Security.Models.Authorization;
using Software.Security.Models.Message;
using System;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Software.Security.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IAuthorizationRepository _authorizationRepository;

        private UserViewModel _user;
        public MessageController(IMessageRepository messageRepository, IAuthorizationRepository authorizationRepository)
        {
            _messageRepository = messageRepository;
            _authorizationRepository = authorizationRepository;
        }

        public ActionResult AddMessage(string text)
        {
            return View(new MessageViewModel()
            {
                Text = text
            });
        }


        public ActionResult EditMessage(string text, int id)
        {
            return View(new MessageViewModel()
            {
                Text = text,
                MessageId = id
            });
        }

        public ActionResult RemoveMessage(int id)
        {
            return View(new MessageViewModel()
            {
                MessageId = id
            });
        }

        public ActionResult AddMessagePost(string text)
        {
            var model = this._messageRepository.AddMessage(text, this._user.UserId);
            //return Json(model, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Index", "Home");

        }

        public ActionResult RemoveMessagePost(int id)
        {
            if (this._authorizationRepository.IsUserOwnerMessage(this._user.UserId, id))
            {
                var model = this._messageRepository.RemoveMessage(id);
                //return Json(model, JsonRequestBehavior.AllowGet);
                return RedirectToAction("Index", "Home");

            }
            throw new UnauthorizedAccessException();
        }
        public ActionResult EditMessagePost(int? messageID, string text)
        {
            var messageId = messageID ?? default(int);
            if (this._authorizationRepository.IsUserOwnerMessage(this._user.UserId, messageId) || this._authorizationRepository.IsUserAllowedToEdit(this._user.UserId, messageId))
            {
                var model = this._messageRepository.EditMessage(messageId, text);
                //return Json(model, JsonRequestBehavior.AllowGet);
                return RedirectToAction("Index","Home");
            }
            throw new UnauthorizedAccessException();
        }

        public ActionResult AddAllowedUser(int messageId)
        {
            var model = new AllowedMessageViewModel();
            model.Message = this._messageRepository.GetMessage(messageId);
            //model.Users = this.
            return View();
        }

        private UserViewModel GetCurrentUser()
        {
            return _user = Session["CurrentUser"] as UserViewModel;
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            //base.OnAuthorization(filterContext);
            _user = Session["CurrentUser"] as UserViewModel;
            if (_user == null) throw new UnauthorizedAccessException();
        }
        protected override void OnAuthentication(AuthenticationContext filterContext)
        {

            //base.OnAuthentication(filterContext);
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
        }

    }

}