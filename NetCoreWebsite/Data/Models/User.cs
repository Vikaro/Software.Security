using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreWebsite.Data.Models
{
    public class User : IdentityUser<int>
    {
        public User()
        {
            this.AllowedMessages = new HashSet<UserMessage>();
            this.LoginLogs = new HashSet<LoginLogs>();
        }

        public int MaxFailedCount { get; set; }
        public virtual ICollection<UserMessage> AllowedMessages { get; set; }
        [InverseProperty("Owner")]
        public virtual ICollection<Message> OwnedMessages { get; set; }

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
