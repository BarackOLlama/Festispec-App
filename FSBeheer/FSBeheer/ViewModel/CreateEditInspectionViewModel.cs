using FSBeheer.Model;
using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class CreateEditInspectionViewModel : ViewModelBase
    {
        private CustomFSContext _context;

        public InspectionVM Inspection { get; set; }
        public ObservableCollection<CustomerVM> Customers { get; }
        public ObservableCollection<EventVM> Events { get; set; }
        public ObservableCollection<StatusVM> Statuses { get; set; }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        private DateTime _startDate { get; set; }
        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
                Inspection.InspectionDate.StartDate = value;
                RaisePropertyChanged(nameof(StartDate));
            }
        }
        private DateTime _endDate { get; set; }
        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
                Inspection.InspectionDate.EndDate = value;
                RaisePropertyChanged(nameof(EndDate));
            }
        }
        private TimeSpan? _startTime;
        public TimeSpan? StartTime {
            get
            {
                return _startTime;
            }
            set
            {
                _startTime = value;
                Inspection.InspectionDate.StartTime = value;
                RaisePropertyChanged(nameof(StartTime));
            }
        }
        private TimeSpan? _endTime { get; set; }
        public TimeSpan? EndTime {
            get
            {
                return _endTime;
            }
            set
            {
                _endTime = value;
                Inspection.InspectionDate.EndTime = value;
                RaisePropertyChanged(nameof(EndTime));
            }
        }
        private EventVM _selectedEvent { get; set; }
        public EventVM SelectedEvent {
            get
            {
                return _selectedEvent;
            }
            set
            {
                _selectedEvent = value;
                if (Inspection != null)
                {
                    Inspection.Event = value.ToModel();
                }
                RaisePropertyChanged(nameof(SelectedEvent));
            }
        }
        private StatusVM _selectedStatus;
        public StatusVM SelectedStatus {
            get
            {
                return _selectedStatus;
            }
            set
            {
                _selectedStatus = value;
                if (Inspection != null)
                {
                    Inspection.Status = value.ToModel();
                }
                RaisePropertyChanged(nameof(SelectedStatus));
            }
        }

        public RelayCommand<Window> CancelInspectionCommand { get; set; }
        public RelayCommand AddInspectionCommand { get; set; }
        public RelayCommand PickInspectorsCommand { get; set; }


        public CreateEditInspectionViewModel()
        {
            // Darjush laten weten
            Messenger.Default.Register<ObservableCollection<InspectorVM>>(this, "UpdateAvailableList", cl => SetInspectors(cl));

            _context = new CustomFSContext();
            Customers = _context.CustomerCrud.GetAllCustomers();
            Events = _context.EventCrud.GetAllEvents();
            Statuses = _context.StatusCrud.GetAllStatusVMs();

            // niet netjes, maar anders heeft de Event combobox in het begin helemaal geen waarde, mede omdat de SetInspection na de InitializeComponent wordt uitgevoerd
            //SelectedEvent = Events.ElementAtOrDefault(2);
            //SelectedStatus = Statuses.ElementAtOrDefault(2);

            CancelInspectionCommand = new RelayCommand<Window>(CancelInspection);
            AddInspectionCommand = new RelayCommand(AddInspection);
            PickInspectorsCommand = new RelayCommand(OpenAvailable);
        }

        public void SetInspectors(ObservableCollection<InspectorVM> SelectedInspectors)
        {
            // TODO: Not in memory/database after reopening the screen

            if (Inspection != null)
            {
                Inspection.Inspectors =  SelectedInspectors;
                RaisePropertyChanged(nameof(Inspection));
            }
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
                _context.Inspections.Add(Inspection.ToModel());
                RaisePropertyChanged(nameof(Inspection));
            }
            else
            {
                Inspection = new InspectionVM(_context.Inspections
                    .FirstOrDefault(i => i.Id == inspection.Id));
                SelectedEvent = _context.EventCrud.GetEventById(Inspection.Event.Id);
                SelectedStatus = _context.StatusCrud.GetStatusById(Inspection.Status.Id);
                StartDate = Inspection.InspectionDate.StartDate;
                EndDate = Inspection.InspectionDate.EndDate;
                StartTime = Inspection.InspectionDate.StartTime;
                EndTime = Inspection.InspectionDate.EndTime;
                RaisePropertyChanged(nameof(Inspection));
            }
        }

        private void CancelInspection(Window window)
        {
            // CreateInspectionView sluiten en veranderingen ongedaan maken
            MessageBoxResult result = MessageBox.Show("Weet u zeker dat u deze inspectie wilt annuleren?", "Bevestig annulering inspectie", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _context.Dispose();
                Inspection = null;
                window.Close();
            }
        }

        private bool InspectionIsValid()
        {
            return true;
        }

        public void AddInspection()
        {
            if (!InspectionIsValid()) return;

            if (IsInternetConnected())
            {
                // Inspectie aanmaken in de database met alle velden die ingevuld zijn
                _context.InspectionCrud.GetAllInspections().Add(Inspection);
                _context.SaveChanges();

                Messenger.Default.Send(true, "UpdateInspectionList");
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        public void OpenAvailable()
        {
            if(IsInternetConnected())
                new AvailableInspectorView(Inspection.Id).Show();
            else
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
        }
    }
}
