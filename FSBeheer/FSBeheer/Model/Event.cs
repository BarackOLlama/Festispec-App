﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Model
{
    public partial class Event
    {
        public Event()
        {
            Inspections = new ObservableCollection<Inspection>();
        }

        [Key]
        public int Id { get; set; }
        [MaxLength(99)]
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public int? EventDateId { get; set; }
        public virtual EventDate EventDate { get; set; }
        public virtual ObservableCollection<Inspection> Inspections { get; set; }
        public bool IsDeleted { get; set; }
    }
}
