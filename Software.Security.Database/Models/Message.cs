using System;

namespace Software.Security.Database.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public DateTime Modified { get; set; }
    }
}
