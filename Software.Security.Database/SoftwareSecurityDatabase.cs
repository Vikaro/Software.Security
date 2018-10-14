using NMemory.Tables;
using Software.Security.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Software.Security.Database
{
    public interface ISoftwareSecurityDatabase {
        ITable<User> Users { get; }

        ITable<Message> Messages { get;  }
    };

    public class SoftwareSecurityDatabase : NMemory.Database, ISoftwareSecurityDatabase
    {
        public SoftwareSecurityDatabase()
        {
            var users = base.Tables.Create<User, Guid>(p => p.UserId, null);
            var messages = base.Tables.Create<Message, Guid>(g => g.MessageId, null);

            InitializationData.InitializeUsers(users);

            this.Users = users;
            this.Messages = messages;
        }

        public ITable<User> Users { get; private set; }

        public ITable<Message> Messages { get; private set; }
    }

    static class InitializationData
    {
        static internal void InitializeUsers(ITable<User> users)
        {
            users.Insert(new User
            {
                Name = "admin",
                PasswordHash = "admin",
                LastLogin = DateTime.Now,
                Salt = string.Empty
            });
        }
    }
}
