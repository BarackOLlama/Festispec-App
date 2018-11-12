using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace FSBeheer.Model
{
    public partial class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Adres { get; set; }
        public string Place { get; set; }
        public string ZipCode { get; set; }
        public DateTime? StartingDate { get; set; }
        public short? ChamberOfCommerceNumber { get; set; }
    }
}
