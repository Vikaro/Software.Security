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
    public class User
    {
        public User()
        {
            this.AllowedMessages = new HashSet<UserMessage>();
            this.LoginLogs = new HashSet<UserLogs>();
        }
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        [Display(Name = "Password")]
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public int MaxFailedCount { get; set; }
        public DateTime LockedUntil { get; set; }
        public bool Locked { get; set; }
        public DateTime LastSuccesfullLogin { get; set; }
        public virtual ICollection<UserMessage> AllowedMessages { get; set; }
        [InverseProperty("Owner")]
        public virtual ICollection<Message> OwnedMessages { get; set; }

        public virtual ICollection<UserLogs> LoginLogs { get; set; }
    }
    public class UserLogs
    {
        [Key]
        public int Id { get; set; }
        public virtual User User { get; set; }
        public DateTime Date { get; set; }
        public bool Successfull { get; set; }
    }
}
