using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Model
{
    public partial class Account
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(99)]
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsAdmin { get; set; }
        public int? RoleId { get; set; }
        public virtual Role Role { get; set; }
        public bool IsDeleted { get; set; }
    }
}
