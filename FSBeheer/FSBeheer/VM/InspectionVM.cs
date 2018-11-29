using FSBeheer.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.VM
{
    public class InspectionVM
    {
        private Inspection _inspection;

        public InspectionVM(Inspection inspection)
        {
            _inspection = inspection;
        }

        // useful?
        internal Inspection ToModel()
        {
            return _inspection;
        }

        public int Id
        {
            get { return _inspection.Id; }
        }

        public string Name
        {
            get { return _inspection.Name; }
        }

        public string Notes
        {
            get { return _inspection.Notes; }
        }

        public int? EventId
        {
            get { return _inspection.EventId; }
        }

        public Event Event
        {
            get { return _inspection.Event; }
        }

        public int? StatusId
        {
            get { return _inspection.StatusId; }
        }

        public string StatusName
        {
            get { return Status.StatusName; }
            set { Status.StatusName = value; }
        }

        public Status Status
        {
            get { return _inspection.Status; }
            set {  }
        }
    }
}
