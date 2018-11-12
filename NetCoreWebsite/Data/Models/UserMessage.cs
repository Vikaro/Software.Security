using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebsite.Data.Models
{
    public class UserMessage
    {
        public int UserId { get; set; }
        public int MessageId { get; set; }
        public User User { get; set; }
        public Message Message { get; set; }
    }
}
