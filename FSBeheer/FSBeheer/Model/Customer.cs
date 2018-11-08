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

        [Required]
        public string Name { get; set; }

        [Required]
        public string Adres { get; set; }

        [Required]
        public string Place { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public DateTime StartingDate { get; set; }

        [Required]
        
        public short ChamberOfCommerceNumber { get; set; }
    }
}
