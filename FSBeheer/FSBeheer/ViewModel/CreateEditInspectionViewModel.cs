using FSBeheer.Model;
using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class CreateEditInspectionViewModel : ViewModelBase
    {
        private CustomFSContext _Context;

        public InspectionVM Inspection { get; set; }
        public ObservableCollection<CustomerVM> Customers { get; }
        public ObservableCollection<EventVM> Events { get; set; }
        public ObservableCollection<StatusVM> Statuses { get; set; }

        public string WarningText { get; set; }
        private DateTime _StartDate { get; set; }
        public DateTime StartDate
        {
            get
            {
                return _StartDate;
            }
            set
            {
                _StartDate = value;
                Inspection.InspectionDate.StartDate = value;
                RaisePropertyChanged(nameof(StartDate));
            }
        }
        private DateTime _EndDate { get; set; }
        public DateTime EndDate
        {
            get
            {
                return _EndDate;
            }
            set
            {
                _EndDate = value;
                Inspection.InspectionDate.EndDate = value;
                RaisePropertyChanged(nameof(EndDate));
            }
        }
        private TimeSpan? _StartTime { get; set; }
        public TimeSpan? StartTime {
            get
            {
                return _StartTime;
            }
            set
            {
                _StartTime = value;
                Inspection.InspectionDate.StartTime = value;
                RaisePropertyChanged(nameof(StartTime));
            }
        }
        private TimeSpan? _EndTime { get; set; }
        public TimeSpan? EndTime {
            get
            {
                return _EndTime;
            }
            set
            {
                _EndTime = value;
                Inspection.InspectionDate.EndTime = value;
                RaisePropertyChanged(nameof(EndTime));
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
                if (Inspection != null)
                {
                    Inspection.Event = value.ToModel();
                }
                RaisePropertyChanged(nameof(SelectedEvent));
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
                if (Inspection != null)
                {
                    Inspection.Status = value.ToModel();
                }
                RaisePropertyChanged(nameof(SelectedStatus));
            }
        }

        //public RelayCommand<Window> CancelInspectionCommand { get; set; }
        //public RelayCommand<Window> AddInspectionCommand { get; set; }
        public RelayCommand AddInspectionCommand { get; set; }
        //public RelayCommand CanExecuteChangedCommand { get; set; }
        public RelayCommand PickInspectorsCommand { get; set; }

        public CreateEditInspectionViewModel()
        {
            _Context = new CustomFSContext();
            Customers = _Context.CustomerCrud.GetAllCustomers();
            Events = _Context.EventCrud.GetAllEvents();
            Statuses = _Context.StatusCrud.GetAllStatusVMs();

            //CancelInspectionCommand = new RelayCommand<Window>(CancelInspection);
            //AddInspectionCommand = new RelayCommand<Window>(AddInspection, CheckSaveAllowed);
            AddInspectionCommand = new RelayCommand(AddInspection);
            //CanExecuteChangedCommand = new RelayCommand(CanExecuteChanged);
            PickInspectorsCommand = new RelayCommand(OpenAvailable);
        }

        public void SetInspection(InspectionVM inspection)
        {
            if (inspection == null)
            {
                Inspection = new InspectionVM(new Inspection())
                {
                    InspectionDate = new InspectionDate()
                };
                StartDate = new DateTime(2000, 01, 01);
                EndDate = new DateTime(2000, 01, 01);
                _Context.Inspections.Add(Inspection.ToModel());
                RaisePropertyChanged(nameof(Inspection));
            }
            else
            {
                Inspection = new InspectionVM(_Context.Inspections
                    .FirstOrDefault(i => i.Id == inspection.Id));
                SelectedEvent = _Context.EventCrud.GetEventById(Inspection.Event.Id);
                SelectedStatus = _Context.StatusCrud.GetStatusById(Inspection.Status.Id);
                StartDate = Inspection.InspectionDate.StartDate;
                EndDate = Inspection.InspectionDate.EndDate;
                StartTime = Inspection.InspectionDate.StartTime;
                EndTime = Inspection.InspectionDate.EndTime;
                RaisePropertyChanged(nameof(Inspection));
            }
        }

        //private void CloseAction(Window window)
        //{
        //    _Context.Dispose();
        //    window.Close();
        //}

        //private void CancelInspection(Window window)
        //{
        //    // CreateInspectionView sluiten en veranderingen ongedaan maken
        //    MessageBoxResult result = MessageBox.Show("Weet u zeker dat u deze inspectie wilt annuleren?", "Bevestig annulering inspectie", MessageBoxButton.OKCancel);
        //    if (result == MessageBoxResult.OK)
        //    {
        //        CloseAction(window);
        //    }
        //}

        //public void AddInspection(Window window)
        //{
        //    _Context.InspectionCrud.GetAllInspectionVMs().Add(Inspection);
        //    _Context.SaveChanges();

        //    Messenger.Default.Send(true, "UpdateInspectionList");
        //    CloseAction(window);
        //}

        public void AddInspection()
        {
            _Context.InspectionCrud.GetAllInspectionVMs().Add(Inspection);
            _Context.SaveChanges();

            Messenger.Default.Send(true, "UpdateInspectionList");
        }

        public void OpenAvailable()
        {
            new AvailableInspectorView(Inspection.Id).Show();
        }

        //private void CanExecuteChanged()
        //{
        //    AddInspectionCommand.RaiseCanExecuteChanged();
        //}

        //private bool CheckSaveAllowed(Window window)
        //{
        //    if (Inspection == null)
        //    {
        //        return false;
        //    }else if (string.IsNullOrEmpty(Inspection.Name))
        //    {
        //        WarningText = "Het veld Naam mag niet leeg zijn";
        //        RaisePropertyChanged(nameof(WarningText));
        //        return false;
        //    }
        //    else if (Inspection.Inspectors == null)
        //    {
        //        WarningText = "Het veld Inspecteur(s) mag niet leeg zijn";
        //        RaisePropertyChanged(nameof(WarningText));
        //        return false;
        //    }
        //    else
        //    {
        //        WarningText = "";
        //        RaisePropertyChanged(nameof(WarningText));
        //        return true;
        //    }
        //}

    }
}
