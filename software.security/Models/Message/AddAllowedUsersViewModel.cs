using Software.Security.Models.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Software.Security.Models.Message
{
    public class AllowedMessageViewModel
    {
        public Database.Models.Message Message { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }
        public int SelectedUser { get; set; }
    }
}