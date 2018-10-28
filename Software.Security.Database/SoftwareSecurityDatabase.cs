using NMemory.Tables;
using Software.Security.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NMemory.Constraints;
using NMemory.Indexes;
using NMemory.Utilities;
using System.Data.Entity;
using System.Linq;

namespace Software.Security.Database
{
  

    public class SoftwareSecurityDatabase : DbContext
    {
      public DbSet<User> Users { get; set; }
      public DbSet<Message> Messages { get; set; }
      //public DbSet<AllowedMessage> AllowedMessages { get; set; }
        public DbContext context;
        public SoftwareSecurityDatabase(): base()
        {
            System.Data.Entity.Database.SetInitializer<SoftwareSecurityDatabase>(new SoftwareSecurityInitializer());

        }

    }
    public class SoftwareSecurityInitializer : DropCreateDatabaseAlways<SoftwareSecurityDatabase>
    {
        protected override void Seed(SoftwareSecurityDatabase context)
        {
            List<Message> defaultStandards = new List<Message>();
            List<User> users = new List<User>();
            users.Add(new User()
            {
                Name = "admin",
                PasswordHash = "admin",
                LastLogin = DateTime.Now,
                Salt = string.Empty,
            });
            users.Add(new User()
            {
                Name = "admin2",
                PasswordHash = "admin2",
                LastLogin = DateTime.Now,
                Salt = string.Empty,
            });
            users.Add(new User()
            {
                Name = "admin3",
                PasswordHash = "admin3",
                LastLogin = DateTime.Now,
                Salt = string.Empty,
            });
            defaultStandards.Add(new Message() { Text = "Standard 1", Owner = users.First(),Modified = DateTime.Now });
            defaultStandards.Add(new Message() { Text = "Standard 2", Owner = users.First(),Modified = DateTime.Now });
            defaultStandards.Add(new Message() { Text = "Standard 3", Owner = users.First(),Modified = DateTime.Now });
            context.Users.AddRange(users);
            context.Messages.AddRange(defaultStandards);

            base.Seed(context);
        }
    }
    static class InitializationData
    {
        private static int  adminId = 1;
        //static internal void InitializeUsers(ITable<User> users)
        //{
        //    users.Insert(new User
        //    {
        //        Name = "admin",
        //        PasswordHash = "admin",
        //        LastLogin = DateTime.Now,
        //        Salt = string.Empty,
        //        UserId = adminId
        //    });
        //    users.Insert(new User
        //    {
        //        Name = "admin2",
        //        PasswordHash = "admin2",
        //        LastLogin = DateTime.Now,
        //        Salt = string.Empty,
        //        UserId = 2
        //    });

        //    users.Insert(new User
        //    {
        //        Name = "admin3",
        //        PasswordHash = "admin3",
        //        LastLogin = DateTime.Now,
        //        Salt = string.Empty,
        //        UserId = 3
        //    });
        //}
        //static internal void InitializeMessages(ITable<Message> messages)
        //{
        //    messages.Insert(new Message
        //    {
        //        Modified = DateTime.Now,
        //        Text = "Random text",
        //        Owner = adminId,
        //    });
        //    messages.Insert(new Message
        //    {
        //        Modified = DateTime.Now.AddHours(-2),
        //        Text = "Random text 2",
        //        Owner = adminId,
        //    });
        //}
        //static internal void InitializeAllowedMessages(ITable<AllowedMessage> allowedMessages)
        //{
        //    allowedMessages.Insert(new AllowedMessage()
        //    {
        //        UserId = 2,
        //        MessageId = 1
        //    });
        //}

    }
}
