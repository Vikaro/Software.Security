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
    }
}