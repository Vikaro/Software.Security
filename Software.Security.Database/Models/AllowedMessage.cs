using System;

namespace Software.Security.Database.Models
{
    public class AllowedMessage
    {
        public Guid UserId { get; set; }
        public Guid MessageId { get; set; }
    }
}
