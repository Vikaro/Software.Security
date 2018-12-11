using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebsite.Data.Models
{
    public class UserSecondPassword
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("userFK")]
        public User User { get; set; }
        public string Mask { get; set; }
        public string Hash { get; set; }
        public bool Removed { get; set; }
    }
}
