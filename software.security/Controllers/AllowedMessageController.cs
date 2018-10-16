using AutoMapper;
using Software.Security.Database.Repository;
using Software.Security.Models.Authorization;
using Software.Security.Models.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Software.Security.Controllers
{
    public class AllowedMessageController : Controller
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IMapper _mapper;

        private UserViewModel _user;

        public AllowedMessageController(IMessageRepository messageRepository, IAuthorizationRepository authorizationRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _authorizationRepository = authorizationRepository;
            _mapper = mapper;
        }


        // GET: AllowedMessage/Create
        public ActionResult Create(int id)
        {
            if (this._authorizationRepository.IsUserOwnerMessage(_user.UserId, id))
            {
                var model = new AllowedMessageViewModel();
                model.Message = this._messageRepository.GetMessage(id);
                var users = this._mapper.Map<IEnumerable<UserViewModel>>(this._authorizationRepository.GetUsers());
                model.Users = users.Select(i => new SelectListItem() { Value = i.UserId.ToString(), Text = i.Name });
                return View(model);
            }
            throw new UnauthorizedAccessException();
        }

        // POST: AllowedMessage/Create
        [HttpGet]
        public ActionResult CreatePost(int selectedUser, int messageId)
        {
            if (this._authorizationRepository.IsUserOwnerMessage(_user.UserId, messageId))
            {
                this._authorizationRepository.AddUserToAllowedMessage(selectedUser, messageId);
                return RedirectToAction("Index", "Home");
            }
            throw new UnauthorizedAccessException();
        }
        // GET: AllowedMessage/Delete/5
        public ActionResult Delete(int id)
        {
            if (this._authorizationRepository.IsUserOwnerMessage(_user.UserId, id))
            {
                var model = new AllowedMessageViewModel();
                model.Message = this._messageRepository.GetMessage(id);
                var users = this._mapper.Map<IEnumerable<UserViewModel>>(this._authorizationRepository.GetUsersFromAllowedMessage(id));
                model.Users = users.Select(i => new SelectListItem() { Value = i.UserId.ToString(), Text = i.Name });
                return View(model);
            }
            throw new UnauthorizedAccessException();

        }

        // POST: AllowedMessage/Delete/5
        [HttpGet]
        public ActionResult DeletePost(int selectedUser, int messageId)
        {
            if (this._authorizationRepository.IsUserOwnerMessage(_user.UserId, messageId))
            {
                this._authorizationRepository.RemoveUserFromAllowedMessage(selectedUser, messageId);
                return RedirectToAction("Index", "Home");
            }
            throw new UnauthorizedAccessException();
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
    }
}
