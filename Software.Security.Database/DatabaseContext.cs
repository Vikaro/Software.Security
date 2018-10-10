using NMemory.Tables;
using Software.Security.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Software.Security.Database
{
    internal class MyDatabase : NMemory.Database
    {
        public MyDatabase()
        {
            var users = base.Tables.Create<User, Guid>(p => p.UserId, null);
            var messages = base.Tables.Create<Message, Guid>(g => g.MessageId, null);
            
            this.Users = users;
            this.Messages = messages;
        }

        internal ITable<User> Users { get; private set; }

        internal ITable<Message> Messages { get; private set; }
    }

    public class DatabaseService
    {

    }
}
