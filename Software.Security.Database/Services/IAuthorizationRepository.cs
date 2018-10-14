using Software.Security.Database.Models;

namespace Software.Security.Database
{
    public interface IAuthorizationRepository
    {
        bool IsUser(string login, string password);
        bool Register(string login, string password);
        User GetUser(string login);
    }
}