using System.Collections.Generic;
using Software.Security.Database.Models;

namespace Software.Security.Database.Repository
{
    public interface IMessageRepository
    {
        Message AddMessage(string text, int userId);
        Message EditMessage(int id, string text);
        IEnumerable<Message> GetMessages();
        bool RemoveMessage(int id);
        Message GetMessage(int id);
    }
}