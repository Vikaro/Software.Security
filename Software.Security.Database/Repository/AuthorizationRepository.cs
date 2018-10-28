using Software.Security.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Software.Security.Database.Repository
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly SoftwareSecurityDatabase _database; 

        public AuthorizationRepository(SoftwareSecurityDatabase database)
        {
            _database = database;
        }

        public bool IsUserExist(string login, string passwordHash)
        {
            return this._database.Users.Where(i => i.Name.Equals(login) && i.PasswordHash.Equals(passwordHash)).Any();
        }

        public bool IsUserOwnerMessage(int userId, int messageId)
        {
            var message = this._database.Messages.Where(i => i.MessageId.Equals(messageId)).FirstOrDefault();
            return message != null ? message.Owner.UserId.Equals(userId): false;
        }

        public bool IsUserAllowedToEdit(int userId, int messageId)
        {

            return this._database.Messages.Where(i => i.MessageId.Equals(messageId) && i.Allowed.Where(j=> j.UserId.Equals(userId)).Any()).Any();
        }

        public bool Register(string login, string passwordHash)
        {
            if(this._database.Users.Any(i=> i.Name.Equals(login)))
            {
                return false;
            }
            this._database.Users.Add(new User()
            {
                Name = login,
                PasswordHash = passwordHash,
            });
            this._database.SaveChanges();
            return true;
        }

        public User GetUser(string login)
        {
            return this._database.Users.Where(i => i.Name.Equals(login)).FirstOrDefault();
        }
        public User GetUser(int userId)
        {
            return this._database.Users.Where(i => i.UserId.Equals(userId)).FirstOrDefault();

        }
        
        public void AddUserToAllowedMessage(int userId, int messageId)
        {
            var message = this._database.Messages.Where(i => i.MessageId.Equals(messageId)).FirstOrDefault();
            if (message != null)
            {
                message.Allowed.Add(this.GetUser(userId));
                this._database.SaveChanges();
            }
        }

        public bool RemoveUserFromAllowedMessage(int userId, int messageId)
        {
            var message = this._database.Messages.Where(i => i.MessageId.Equals(messageId)).FirstOrDefault();
            if (message != null)
            {
                message.Allowed.Remove(this.GetUser(userId));
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
                    yield return this._database.Users.Where(i => i.UserId.Equals(item.UserId)).FirstOrDefault();
                }
            }
        }
        public IEnumerable<User> GetUsers()
        {
            return this._database.Users.ToList();
        }
    }
}
