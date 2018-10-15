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
        ITable<AllowedMessage> AllowedMessages { get; }
    };

    public class SoftwareSecurityDatabase : NMemory.Database, ISoftwareSecurityDatabase
    {
        public SoftwareSecurityDatabase()
        {
            var users = base.Tables.Create(p => p.UserId, new IdentitySpecification<User>(x => x.UserId,1, 1 ));
            var messages = base.Tables.Create(g => g.MessageId, new IdentitySpecification<Message>(x => x.MessageId, 1, 1));
            var allowedMessages = base.Tables.Create(g => g.MessageId, new IdentitySpecification<AllowedMessage>(x => x.MessageId, 1, 1));

            InitializationData.InitializeUsers(users);
            InitializationData.InitializeMessages(messages);
            InitializationData.InitializeAllowedMessages(allowedMessages);
            
            this.Users = users;
            this.Messages = messages;
            this.AllowedMessages = allowedMessages;
        }

        public ITable<User> Users { get; private set; }

        public ITable<Message> Messages { get; private set; }
        public ITable<AllowedMessage> AllowedMessages { get; private set; }
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
                UserId = adminId
            });
            users.Insert(new User
            {
                Name = "subadmin",
                PasswordHash = "admin",
                LastLogin = DateTime.Now,
                Salt = string.Empty,
                UserId = 2
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
                Modified = DateTime.Now.AddHours(-2),
                Text = "Random text 2",
                UserId = adminId,
            });
        }
        static internal void InitializeAllowedMessages(ITable<AllowedMessage> allowedMessages)
        {
            allowedMessages.Insert(new AllowedMessage()
            {
                UserId = 2,
                MessageId = 1
            });
        }

    }
}
