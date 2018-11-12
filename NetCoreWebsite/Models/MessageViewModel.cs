using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreWebsite.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebsite.Models
{
    public class MessageViewModel
    {
        public Message Message { get; set; }
        public IEnumerable<string> SelectedUsers { get; set; }
        public virtual IEnumerable<SelectListItem> UserList { get; set; }
    }
}
