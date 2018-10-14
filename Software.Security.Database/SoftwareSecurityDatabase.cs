using NMemory.Tables;
using Software.Security.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NMemory.Constraints;
using NMemory.Indexes;
using NMemory.Utilities;

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
            var users = base.Tables.Create(p => p.UserId, new IdentitySpecification<User>(x => x.UserId,1, 1 ));
            var messages = base.Tables.Create(g => g.MessageId, new IdentitySpecification<Message>(x => x.MessageId, 1, 1));

            InitializationData.InitializeUsers(users);
            InitializationData.InitializeMessages(messages);

            this.Users = users;
            this.Messages = messages;
        }

        public ITable<User> Users { get; private set; }

        public ITable<Message> Messages { get; private set; }
    }

    static class InitializationData
    {
        private static int  adminId = 1;
        static internal void InitializeUsers(ITable<User> users)
        {
            users.Insert(new User
            {
                Name = "admin",
                PasswordHash = "admin",
                LastLogin = DateTime.Now,
                Salt = string.Empty,
            });
        }
        static internal void InitializeMessages(ITable<Message> messages)
        {
            messages.Insert(new Message
            {
                Modified = DateTime.Now,
                Text = "Random text",
                UserId = adminId,
            });
            messages.Insert(new Message
            {
                Modified = DateTime.Now,
                Text = "Random text 2",
                UserId = adminId,
            });
        }
    }
}
