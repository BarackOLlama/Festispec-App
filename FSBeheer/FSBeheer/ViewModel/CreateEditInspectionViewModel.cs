using FSBeheer.Model;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace FSBeheer.ViewModel
{
    public class CreateEditInspectionViewModel : ViewModelBase
    {
        private CustomFSContext _Context;

        public ObservableCollection<CustomerVM> Customers { get; }
        public ObservableCollection<EventVM> Events { get; set; }
        //public ObservableCollection<string> EventNames { get; set; }
        public ObservableCollection<StatusVM> Statuses { get; set; }
        //public ObservableCollection<string> StatusNames { get; set; }
        
        //public DateTime? NewStartDate { get; set; }
        //public DateTime? NewEndDate { get; set; }
        //public TimeSpan? NewStartTime { get; set; }
        //public TimeSpan? NewEndTime { get; set; }
        private DateTime? _StartDate { get; set; }
        public DateTime? StartDate
        {
            get
            {
                return _StartDate;
            }
            set
            {
                _StartDate = value;
                //Inspection.InspectionDate.StartDate = value;
            }
        }
        private EventVM _SelectedEvent { get; set; }
        public EventVM SelectedEvent {
            get
            {
                return _SelectedEvent;
            }
            set
            {
                _SelectedEvent = value;
                Inspection.Event = value.ToModel();
            }
        }
        private StatusVM _SelectedStatus { get; set; }
        public StatusVM SelectedStatus {
            get
            {
                return _SelectedStatus;
            }
            set
            {
                _SelectedStatus = value;
                Inspection.Status = _SelectedStatus.ToModel();
            }
        }
        public InspectionVM Inspection { get; set; }
        public RelayCommand CancelInspectionCommand { get; set; }
        public RelayCommand AddInspectionCommand { get; set; }

        public CreateEditInspectionViewModel()
        {
            _Context = new CustomFSContext();
            Customers = _Context.CustomerCrud.GetAllCustomerVMs();
            Events = _Context.EventCrud.GetAllEvents();
            Statuses = _Context.StatusCrud.GetAllStatusVMs();

            CancelInspectionCommand = new RelayCommand(CancelInspection);
            AddInspectionCommand = new RelayCommand(AddInspection);
        }

        public void SetInspection(InspectionVM inspection)
        {
            if (inspection == null)
            {
                Inspection = new InspectionVM(new Inspection());
                _Context.Inspections.Add(Inspection.ToModel());
                RaisePropertyChanged(nameof(Inspection));
            }
            else
            {
                Inspection = new InspectionVM(_Context.Inspections
                    .FirstOrDefault(i => i.Id == inspection.Id));
                RaisePropertyChanged(nameof(Inspection));
            }
        }

        public void AddInspection()
        {
            // Inspectie aanmaken in de database met alle velden die ingevuld zijn
            _Context.InspectionCrud.GetAllInspectionVMs().Add(Inspection);
            _Context.SaveChanges();

            Messenger.Default.Send(true, "UpdateInspectionList");
        }

        public void CancelInspection()
        {
            // CreateInspectionView sluiten en veranderingen ongedaan maken
        }
    }
}
