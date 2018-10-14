﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Software.Security.Database.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public DateTime LastLogin { get; set; }
    }
}