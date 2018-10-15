using Software.Security.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Software.Security.Models.Authorization
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime LastLogin { get; set; }
        //public IEnumerable<MessageViewModel> OwnerMessages { get; set; }
        //public IEnumerable<AllowedMessage> AllowedMessages { get; set; }
    }
}