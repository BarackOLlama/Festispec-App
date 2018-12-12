using FSBeheer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.VM
{
    public class InspectionDateVM
    {
        private InspectionDate _InspectionDate;

        public InspectionDateVM(InspectionDate inspectionDate)
        {
            _InspectionDate = inspectionDate;
        }

        public int Id
        {
            get { return _InspectionDate.Id; }
        }

        public DateTime StartDate
        {
            get { return _InspectionDate.StartDate; }
            set { _InspectionDate.StartDate = value; }
        }

        public DateTime EndDate
        {
            get { return _InspectionDate.EndDate; }
            set { _InspectionDate.EndDate = value; }
        }

        public TimeSpan? StartTime
        {
            get { return _InspectionDate.StartTime; }
            set { _InspectionDate.StartTime = value; }
        }

        public TimeSpan? EndTime
        {
            get { return _InspectionDate.EndTime; }
            set { _InspectionDate.EndTime = value; }
        }

        internal InspectionDate ToModel()
        {
            return _InspectionDate;
        }
    }
}
