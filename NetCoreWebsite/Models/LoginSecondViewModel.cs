using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreWebsite.Models
{
    public class LoginSecondViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public IDictionary<int, MaskViewModel> PasswordMask { get; set; }
        public class MaskViewModel
        {
            [Display(Name = "Character", AutoGenerateField = true)]
            public string Char { get; set; } = "";
            public bool Mask { get; set; } = false;
        }
    }
}
