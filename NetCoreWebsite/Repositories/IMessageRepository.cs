using System.Collections.Generic;
using NetCoreWebsite.Data.Models;

namespace NetCoreWebsite.Repositories
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