using FSBeheer.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.VM
{
    public class ScheduleItemVM : ViewModelBase
    {
        private ScheduleItem _ScheduleItem;

        public ScheduleItemVM(ScheduleItem scheduleItem = null)
        {
            if (scheduleItem != null)
                _ScheduleItem = scheduleItem;
            else
                _ScheduleItem = new ScheduleItem();
            
        }

        public int Id
        {
            get { return _ScheduleItem.Id; }
        }

        public DateTime? Date
        {
            get { return _ScheduleItem.Date; }
            set { _ScheduleItem.Date = value; RaisePropertyChanged(nameof(Date)); }
        }

        public string DateString
        {
            get { return ((DateTime)Date).ToShortDateString(); }
        }

        public TimeSpan? AvailableStartTime
        {
            get { return _ScheduleItem.AvailableStartTime; }
        }

        public TimeSpan? AvailableEndTime
        {
            get { return _ScheduleItem.AvailableEndTime; }
        }

        public TimeSpan? ScheduleStartTime
        {
            get { return _ScheduleItem.ScheduleStartTime; }
            set { _ScheduleItem.ScheduleStartTime = value; RaisePropertyChanged(nameof(ScheduleStartTime)); }
        }

        public TimeSpan? ScheduleEndTime
        {
            get { return _ScheduleItem.ScheduleEndTime; }
            set { _ScheduleItem.ScheduleEndTime = value; RaisePropertyChanged(nameof(ScheduleEndTime)); }
        }

        public bool Scheduled
        {
            get { return _ScheduleItem.Scheduled; }
            set { _ScheduleItem.Scheduled = value; RaisePropertyChanged(nameof(Scheduled)); }
        }

        public string ScheduledString
        {
            get { if (Scheduled) return "Inspectie"; else return "Verlof"; }
        }

        public virtual InspectorVM Inspector
        {
            get { return new InspectorVM(_ScheduleItem.Inspector); }
            set { _ScheduleItem.Inspector = value.ToModel(); }
        }

        internal ScheduleItem ToModel()
        {
            return _ScheduleItem;
        }

        public bool IsDeleted
        {
            get { return _ScheduleItem.IsDeleted; }
            set { _ScheduleItem.IsDeleted = value; }
        }
    }
}
