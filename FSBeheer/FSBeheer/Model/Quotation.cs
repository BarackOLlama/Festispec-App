using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Model
{
    public partial class Quotation
    {
        [Key]
        public int Id { get; set; }
        public decimal? Price { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public int? InspectionId { get; set; }
        public virtual Inspection Inspection { get; set; }
    }
}
