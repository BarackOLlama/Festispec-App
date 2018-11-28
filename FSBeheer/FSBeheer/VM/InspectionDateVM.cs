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
        }

        public DateTime EndDate
        {
            get { return _InspectionDate.EndDate; }
        }

        public TimeSpan? StartTime
        {
            get { return _InspectionDate.StartTime; }
        }

        public TimeSpan? EndTime
        {
            get { return _InspectionDate.EndTime; }
        }
    }
}
