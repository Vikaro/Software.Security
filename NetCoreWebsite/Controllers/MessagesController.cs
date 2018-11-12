using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetCoreWebsite.Data;
using NetCoreWebsite.Data.Models;
using NetCoreWebsite.Models;
using NetCoreWebsite.Repositories;

namespace NetCoreWebsite.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationRepository _authorizationRepository;

        public MessagesController(ApplicationDbContext context, IAuthorizationRepository authorizationRepository)
        {
            _context = context;
            _authorizationRepository = authorizationRepository;
        }

        // GET: Messages
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Messages.Include(i => i.Owner).ToListAsync());
        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
            .FirstOrDefaultAsync(m => m.MessageId == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Messages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MessageId,Text,Modified")] Message message)
        {
            if (ModelState.IsValid)
            {
                message.Owner = await this._context.Users.Where(i => i.Id.Equals(this.GetUserId())).FirstOrDefaultAsync();
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(message);
        }

        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!this.UserAllowedToEdit((int)id)) return this.Unauthorized();

            var message = await _context.Messages.Include(i => i.Allowed).Include(i => i.Owner).Where(i => i.MessageId == id).FirstOrDefaultAsync();
            if (message == null)
            {
                return NotFound();
            }
            var model = new MessageViewModel();
            model.Message = message;
            model.UserList = (await _context.Users.ToListAsync()).Select(i => new SelectListItem { Text = i.UserName, Value = i.Id.ToString(), Selected = message.Allowed.Any(a => a.UserId.Equals(i.Id)) });
            return View(model);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpGet("EditPOST/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST(int id, MessageViewModel model)
        {
            if (model?.Message?.MessageId == default(int))
            {
                return NotFound();
            }
            if (!this.IsUserOwner(model.Message.MessageId)) return this.Unauthorized();

            if (ModelState.IsValid)
            {
                try
                {
                    var allowedUsers = model.SelectedUsers.Select(i => new UserMessage
                    {
                        UserId = int.Parse(i),
                        MessageId = model.Message.MessageId
                    }).ToList();
                    var dbMessage = _context.Messages.Include(i => i.Allowed).Where(i => i.MessageId.Equals(model.Message.MessageId)).FirstOrDefault();
                    _context.RemoveRange(dbMessage.Allowed);
                    await _context.SaveChangesAsync();

                    _context.AddRange(allowedUsers);
                    dbMessage.Modified = DateTime.Now;
                    dbMessage.Text = model.Message.Text;
                    _context.Update(dbMessage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(model.Message.MessageId))
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
            return View(model);
        }
        public async Task<IActionResult> EditMessage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!this.UserAllowedToEdit((int)id)) return this.Unauthorized();

            var message = await _context.Messages.Where(i => i.MessageId == id).FirstOrDefaultAsync();
            if (message == null)
            {
                return NotFound();
            }
            return View(message);
        }
        public async Task<IActionResult> EditMessagePOST(int id, Message model)
        {
            if (model?.MessageId == default(int))
            {
                return NotFound();
            }
            if (!this.UserAllowedToEdit(model.MessageId)) return this.Unauthorized();

            if (ModelState.IsValid)
            {
                try
                {
                   
                    var dbMessage = _context.Messages.Where(i => i.MessageId.Equals(model.MessageId)).FirstOrDefault();
                    dbMessage.Modified = DateTime.Now;
                    dbMessage.Text = model.Text;
                    _context.Update(dbMessage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(model.MessageId))
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
            return View(model);
        }
        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!this.IsUserOwner((int)id)) return this.Unauthorized();

            var message = await _context.Messages
                .FirstOrDefaultAsync(m => m.MessageId == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpGet, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!this.IsUserOwner((int)id)) return this.Unauthorized();

            var message = await _context.Messages.FindAsync(id);
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.MessageId == id);
        }

        private int GetUserId()
        {
            int.TryParse(this.User.FindFirst(ClaimTypes.NameIdentifier).Value, out int userId);
            return userId;
        }
        private bool UserAllowedToEdit(int messageId) => this._authorizationRepository.IsUserOwnerMessage(this.GetUserId(), messageId) || this._authorizationRepository.IsUserAllowedToEdit(this.GetUserId(), messageId);
        private bool IsUserOwner(int messageId) => this._authorizationRepository.IsUserOwnerMessage(this.GetUserId(), messageId);
    }
}
