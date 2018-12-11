using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebsite.Models
{
    public class SecondPasswordChangeViewModel
    {
        [Required]
        [StringLength(16, MinimumLength = 8 )]
        public string Password { get; set; }
    }
}
