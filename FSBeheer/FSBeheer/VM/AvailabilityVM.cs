using FSBeheer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.VM
{
    public class AvailabilityVM
    {
        private Availability _Availability;

        public AvailabilityVM(Availability availability)
        {
            _Availability = availability;
        }

        public int Id
        {
            get { return _Availability.Id; }
        }

        public DateTime? Date
        {
            get { return _Availability.Date; }
        }

        public bool Scheduled
        {
            get { return _Availability.Scheduled; }
        }

        public TimeSpan? AvailableStartTime
        {
            get { return _Availability.AvailableStartTime; }
        }

        public TimeSpan? AvailableEndTime
        {
            get { return _Availability.AvailableEndTime; }
        }

        public TimeSpan? ScheduleStartTime
        {
            get { return _Availability.ScheduleStartTime; }
        }

        public TimeSpan? ScheduleEndTime
        {
            get { return _Availability.ScheduleEndTime; }
        }

        public int? InspectorId
        {
            get { return _Availability.InspectorId; }
        }

        // not sure about this one
        public virtual Inspector Inspector
        {
            get { return _Availability.Inspector; }
        }

        public bool IsDeleted
        {
            get { return _Availability.IsDeleted; }
        }
    }
}
