using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Software.Security.Database.Models
{
    public class Message
    {
        public Message()
        {
            this.Allowed = new HashSet<User>();
        }

        [Key]
        public int MessageId { get; set; }
        public User Owner { get; set; }
        public string Text { get; set; }
        public DateTime Modified { get; set; }

        public virtual ICollection<User> Allowed { get; set; } 
    }
}
