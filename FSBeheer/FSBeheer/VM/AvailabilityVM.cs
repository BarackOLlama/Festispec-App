using FSBeheer.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.VM
{
    public class AvailabilityVM : ViewModelBase
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
            set { _Availability.Date = value; RaisePropertyChanged(nameof(Date)); }
        }

        public string DateString
        {
            get { return ((DateTime)Date).ToShortDateString(); }
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
            set { _Availability.ScheduleStartTime = value; RaisePropertyChanged(nameof(ScheduleStartTime)); }
        }

        public TimeSpan? ScheduleEndTime
        {
            get { return _Availability.ScheduleEndTime; }
            set { _Availability.ScheduleEndTime = value; RaisePropertyChanged(nameof(ScheduleEndTime)); }
        }

        public bool Scheduled
        {
            get { return _Availability.Scheduled; }
            set { _Availability.Scheduled = value; RaisePropertyChanged(nameof(Scheduled)); }
        }

        public string ScheduledString
        {
            get { if (Scheduled) return "Inspectie"; else return "Verlof"; }
        }

        public int? InspectorId
        {
            get { return _Availability.InspectorId; }
        }

        // not sure about this one
        public virtual Inspector Inspector
        {
            get { return _Availability.Inspector; }
            set { _Availability.Inspector = value; RaisePropertyChanged(nameof(Inspector)); }
        }

        internal Availability ToModel()
        {
            return _Availability;
        }

        public bool IsDeleted
        {
            get { return _Availability.IsDeleted; }
        }
    }
}
