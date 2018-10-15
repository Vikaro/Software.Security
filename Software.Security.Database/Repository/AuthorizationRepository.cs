using Software.Security.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Software.Security.Database.Repository
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly ISoftwareSecurityDatabase _database; 

        public AuthorizationRepository(ISoftwareSecurityDatabase database)
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
            return message != null ? message.UserId.Equals(userId): false;
        }

        public bool IsUserAllowedToEdit(int userId, int messageId)
        {
            return this._database.AllowedMessages.Where(i => i.MessageId.Equals(messageId) && i.UserId.Equals(userId)).Any();
        }

        public bool Register(string login, string passwordHash)
        {
            if(this._database.Users.Any(i=> i.Name.Equals(login)))
            {
                throw new ArgumentException();
            }
            this._database.Users.Insert(new User()
            {
                Name = login,
                PasswordHash = passwordHash,
            });
            return true;
        }

        public User GetUser(string login)
        {
            return this._database.Users.Where(i => i.Name.Equals(login)).FirstOrDefault();
        }

        public void AddUserToAllowedMessage(int userId, int messageId)
        {
            this._database.AllowedMessages.Insert(new AllowedMessage()
            {
                MessageId = messageId,
                UserId = userId
            });
        }

        public bool RemoveUserFromAllowedMessage(int userId, int messageId)
        {
            var message = this._database.AllowedMessages.Where(i => i.MessageId.Equals(messageId) && i.UserId.Equals(userId)).FirstOrDefault();
            if(message != null)
            {
                this._database.AllowedMessages.Delete(message);
                return true;
            }
            return false;
        }

        public IEnumerable<User> GetUsersFromAllowedMessage(int messageId)
        {
            var message = this._database.AllowedMessages.Where(i => i.MessageId.Equals(messageId)).ToList();
            if(message != null)
            {
                foreach (var item in message)
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
