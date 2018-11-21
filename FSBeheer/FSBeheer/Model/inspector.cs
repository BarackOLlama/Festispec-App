using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace FSBeheer.Model
{
    public partial class Inspector
    {
        public Inspector()
        {
            Inspections = new ObservableCollection<Inspection>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? CertificationDate { get; set; }
        public DateTime? InvalidDate { get; set; }
        public string BankNumber { get; set; }
        public int? AccountId { get; set; }
        public virtual Account Account { get; set; }
        public virtual ObservableCollection<Inspection> Inspections { get; set; }
        public bool IsDeleted { get; set; }
    }
}
