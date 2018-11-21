using System;
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
            EventDates = new ObservableCollection<EventDate>();
            Inspections = new ObservableCollection<Inspection>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ObservableCollection<EventDate> EventDates { get; set; }
        public virtual ObservableCollection<Inspection> Inspections { get; set; }
        public bool IsDeleted { get; set; }
    }
}
