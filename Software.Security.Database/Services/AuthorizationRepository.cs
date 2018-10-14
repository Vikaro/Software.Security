using Software.Security.Database.Models;
using System;
using System.Linq;

namespace Software.Security.Database
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly ISoftwareSecurityDatabase _database; 

        public AuthorizationRepository(ISoftwareSecurityDatabase database)
        {
            _database = database;
        }

        public bool Login(string login, string password)
        {
            return this._database.Users.Where(i => i.Name.Equals(login) && i.PasswordHash.Equals(password)).Any();
        }

        public bool Register(string login, string password)
        {
            if(this._database.Users.Any(i=> i.Name.Equals(login)))
            {
                throw new ArgumentException();
            }
            this._database.Users.Insert(new User()
            {
                Name = login,
                PasswordHash = password,
            });
            return true;
        }
    }
}
