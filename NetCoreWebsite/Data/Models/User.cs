using Microsoft.AspNetCore.Identity;
using NetCoreWebsite.Manager;
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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Display(Name = "Password")]
        [Required]
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public int MaxFailedCount { get; set; }
        public bool Locked { get; set; }
        public DateTime LastSuccesfullLogin { get; set; }
        //public string PasswordMask { get; set; }
        public int SecondPasswordId { get; set; }
        public ICollection<UserSecondPassword> SecondPassword { get; set; }
        public virtual ICollection<UserMessage> AllowedMessages { get; set; }
        [InverseProperty("Owner")]
        public virtual ICollection<Message> OwnedMessages { get; set; }
        public virtual ICollection<UserLogs> LoginLogs { get; set; }

        public string PasswordMask()
        {
            return this.SecondPassword.FirstOrDefault(i => i.Id == this.SecondPasswordId && i.Removed == false)?.Mask;
        }
        public string SecondPasswordHash()
        {
            return this.SecondPassword.FirstOrDefault(i => i.Id == this.SecondPasswordId && i.Removed == false)?.Hash;
        }
    }

    public class UserLogs
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public virtual User User { get; set; }
        public DateTime Date { get; set; }
        public bool Successfull { get; set; }
        public SignInStep Step { get; set; }
    }

    public struct UserLogPlace
    {
        public const string FirstLogin = "First login";
        public const string SecondLogin = "Second login";
        public const string PasswordChange = "Password change";
    }
}
