using Software.Security.Database.Models;

namespace Software.Security.Database.Repository

{
    public interface IAuthorizationRepository
    {
        bool IsUserExist(string login, string password);
        bool Register(string login, string password);
        User GetUser(string login);
        bool IsUserOwnerMessage(int userId, int messageId);
        bool IsUserAllowedToEdit(int userId, int messageId);
    }
}