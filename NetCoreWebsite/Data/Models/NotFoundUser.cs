using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebsite.Data.Models
{
    public class NotFoundUser
    {

        public int Id { get; set; }
        public string Username { get; set; }
        public int MaxFailedCount { get; set; }
        public string Mask { get; set; }
    }
}
