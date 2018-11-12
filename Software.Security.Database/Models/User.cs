using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Software.Security.Database.Models
{
    public class User
    {
        public User()
        {
            this.AllowedMessages = new HashSet<Message>();
            this.LoginLogs = new HashSet<LoginLogs>();
        }

        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public DateTime LastLogin { get; set; }
        public int MaxFailedCount { get; set; }
        public virtual ICollection<Message> AllowedMessages { get; set; }
        public virtual ICollection<LoginLogs> LoginLogs { get; set; }
    }
    public class LoginLogs
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public bool Successfull { get; set; }
    }
}
