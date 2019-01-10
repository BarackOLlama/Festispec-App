using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace FSBeheer.Model
{
    public partial class Customer
    {
        public Customer()
        {
            Events = new ObservableCollection<Event>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public DateTime? StartingDate { get; set; }
        public decimal? ChamberOfCommerceNumber { get; set; }
        public int ContactId { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual ObservableCollection<Event> Events { get; set; }
        public bool IsDeleted { get; set; }
    }
}
