namespace Software.Security.Database
{
    public interface IAuthorizationRepository
    {
        bool Login(string login, string password);
        bool Register(string login, string password);
    }
}