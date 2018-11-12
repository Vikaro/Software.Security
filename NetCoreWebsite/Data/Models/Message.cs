using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreWebsite.Data.Models
{
    public class Message
    {
        public Message()
        {
            this.Allowed = new HashSet<UserMessage>();
        }

        [Key]
        public int MessageId { get; set; }
        public User Owner { get; set; }
        public string Text { get; set; }
        public DateTime Modified { get; set; }

        public virtual ICollection<UserMessage> Allowed { get; set; } 
    }
}
