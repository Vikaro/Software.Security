using NetCoreWebsite.Data;
using NetCoreWebsite.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreWebsite.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _database;

        private readonly int _messageLimit = 10;
        public MessageRepository(ApplicationDbContext database)
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
                Owner = this._database.Users.Where(i => i.Id.Equals(userId)).FirstOrDefault(),
                Modified = DateTime.Now
            };
            this._database.Messages.Add(message);
            this._database.SaveChanges();
            return message;
        }
        public bool RemoveMessage(int id)
        {
            var message = this._database.Messages.Where(i => i.MessageId.Equals(id)).FirstOrDefault();
            if (message != null)
            {
                this._database.Messages.Remove(message);
                this._database.SaveChanges();
                return true;
            }
            return false;
        }
        public Message EditMessage(int id, string text)
        {
            var message = this._database.Messages.Where(i => i.MessageId.Equals(id)).FirstOrDefault();
            if (message != null)
            {
                message.Text = text;
                message.Modified = DateTime.Now;
                this._database.SaveChanges();
                return message;
            }
            return null;
        }
    }
}
