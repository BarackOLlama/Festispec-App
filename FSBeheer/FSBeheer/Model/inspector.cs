using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Model
{
    public partial class Inspector
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Adres { get; set; }
        [Required]
        public string Place { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int PhoneNumber { get; set; }
        [Required]
        public DateTime CertificateDate { get; set; }
        [Required]
        public DateTime InvalidDate { get; set; }
        [Required]
        public int AccountNumber { get; set; }

        public Account Username { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
