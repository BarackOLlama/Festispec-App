using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Model
{
    public partial class Availability
    {
        [Key]
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public bool Scheduled { get; set; }
        public TimeSpan? AvailableStartTime { get; set; }
        public TimeSpan? AvailableEndTime { get; set; }
        public TimeSpan? ScheduleStartTime { get; set; }
        public TimeSpan? ScheduleEndTime { get; set; }
        public int? InspectorId { get; set; }
        public virtual Inspector Inspector { get; set; }

    }
}
