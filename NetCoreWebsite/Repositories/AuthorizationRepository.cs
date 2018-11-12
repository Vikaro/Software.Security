using Microsoft.EntityFrameworkCore;
using NetCoreWebsite.Data;
using NetCoreWebsite.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreWebsite.Repositories
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly ApplicationDbContext _database; 

        public AuthorizationRepository(ApplicationDbContext database)
        {
            _database = database;
        }

        public bool IsUserExist(string login, string passwordHash)
        {
            return this._database.Users.Where(i => i.UserName.Equals(login) && i.PasswordHash.Equals(passwordHash)).Any();
        }

        public bool IsUserOwnerMessage(int userId, int messageId)
        {
            var message = this._database.Messages.Include(i=> i.Owner).Where(i => i.MessageId.Equals(messageId)).FirstOrDefault();
            return message != null ? message.Owner.Id.Equals(userId): false;
        }

        public bool IsUserAllowedToEdit(int userId, int messageId)
        {
            return this._database.Messages.Include(i=> i.Allowed).Where(i => i.MessageId.Equals(messageId) && i.Allowed.Where(j=> j.UserId.Equals(userId)).Any()).Any();
        }

        public bool Register(string login, string passwordHash)
        {
            if(this._database.Users.Any(i=> i.UserName.Equals(login)))
            {
                return false;
            }
            this._database.Users.Add(new User()
            {
                UserName = login,
                PasswordHash = passwordHash,
            });
            this._database.SaveChanges();
            return true;
        }

        public User GetUser(string login)
        {
            return this._database.Users.Where(i => i.UserName.Equals(login)).FirstOrDefault();
        }
        public User GetUser(int userId)
        {
            return this._database.Users.Where(i => i.UserName.Equals(userId)).FirstOrDefault();

        }
        
        public void AddUserToAllowedMessage(int userId, int messageId)
        {
            var message = this._database.Messages.Where(i => i.MessageId.Equals(messageId)).FirstOrDefault();
            if (message != null)
            {
                message.Allowed.Add(new UserMessage
                {
                    Message = message,
                    User = this.GetUser(userId)
                });
                this._database.SaveChanges();
            }
        }

        public bool RemoveUserFromAllowedMessage(int userId, int messageId)
        {
            var message = this._database.Messages.Where(i => i.MessageId.Equals(messageId)).FirstOrDefault();
            if (message != null)
            {
                var userMessage = message.Allowed.Where(i => i.UserId.Equals(userId)).FirstOrDefault();
                if(userMessage != null)
                {
                    message.Allowed.Remove(userMessage);
                }
                this._database.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<User> GetUsersFromAllowedMessage(int messageId)
        {
            var message = this._database.Messages.Where(i => i.MessageId.Equals(messageId)).FirstOrDefault();

            if (message != null)
            {
                foreach (var item in message.Allowed)
                {
                    yield return this._database.Users.Where(i => i.Id.Equals(item.UserId)).FirstOrDefault();
                }
            }
        }
        public IEnumerable<User> GetUsers()
        {
            return this._database.Users.ToList();
        }
    }
}
