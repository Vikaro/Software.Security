using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Software.Security.Database.Models
{
    internal class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public DateTime LastLogin { get; set; }
    }
    internal class Message
    {
        public Guid MessageId { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; }
        public DateTime Modified { get; set; }
    }
    internal class AllowedMessage
    {
        public Guid UserId { get; set; }
        public Guid MessageId { get; set; }
    }
}
