using Software.Security.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Software.Security.Database.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ISoftwareSecurityDatabase _database;

        private readonly int _messageLimit = 10;
        public MessageRepository(ISoftwareSecurityDatabase database)
        {
            _database = database;
        }
        public IEnumerable<Message> GetMessages()
        {
            return this._database.Messages.Take(_messageLimit).ToList();
        }
        public Message GetMessage(int id)
        {
            return this._database.Messages.Where(i => i.MessageId.Equals(id)).FirstOrDefault();
        }
        public Message AddMessage(string text, int userId)
        {
            var message = new Message()
            {
                Text = text,
                UserId = userId,
                Modified = DateTime.Now
            };
            this._database.Messages.Insert(message);
            return message;
        }
        public bool RemoveMessage(int id)
        {
            var message = this._database.Messages.Where(i => i.MessageId.Equals(id)).FirstOrDefault();
            if(message != null)
            {
                this._database.Messages.Delete(message);
                return true;
            }
            return false;
        }
        public Message EditMessage(int id, string text)
        {
            var message = this._database.Messages.Where(i => i.MessageId.Equals(id)).FirstOrDefault();
            if(message != null)
            {
                message.Text = text;
                message.Modified = DateTime.Now;
                this._database.Messages.Update(message);
                return message;
            }
            return null;
        }
    }
}
