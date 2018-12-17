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
    public class InspectionVM : ViewModelBase
    {
        private Inspection _inspection;

        public InspectionVM(Inspection inspection)
        {
            _inspection = inspection;            
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

        public EventVM Event
        {
            get { return new EventVM(_inspection.Event); }
            set { Event = value; }
        }

        public int? StatusId
        {
            get { return _inspection.StatusId; }
        }

        public StatusVM Status
        {
            get { return new StatusVM(_inspection.Status); }
            set { _inspection.Status = value.ToModel(); }
        }

        public InspectionDateVM InspectionDate
        {
            get { return new InspectionDateVM(_inspection.InspectionDate); }
            set { _inspection.InspectionDate = value.ToModel(); }
        }

        public ObservableCollection<InspectorVM> Inspectors
        {
            get { return new ObservableCollection<InspectorVM>(_inspection.Inspectors.Select(i => new InspectorVM(i))); }
            set { _inspection.Inspectors = new ObservableCollection<Inspector>(value.Select(i => i.ToModel())); }
        }

        public ObservableCollection<InspectorVM> Inspectors
        {
            get
            {
                return new ObservableCollection<InspectorVM>(_inspection.Inspectors.ToList().Select(i => new InspectorVM(i))); 
            }
            set
            {
                _inspection.Inspectors = new ObservableCollection<Inspector>(value.ToList().Select(i => i.ToModel()));
            }
        }

        public bool IsDeleted
        {
            get { return _inspection.IsDeleted; }
            set { _inspection.IsDeleted = value; RaisePropertyChanged(nameof(IsDeleted)); }
        }

        internal Inspection ToModel()
        {
            return _inspection;
        }
    }
}
