using FSBeheer.Model;
using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class CreateEditInspectionViewModel : ViewModelBase
    {
        private CustomFSContext _context;

        private InspectionVM _inspection;
        public InspectionVM Inspection
        {
            get { return _inspection; }
            set
            {
                _inspection = value;
                RaisePropertyChanged(nameof(Inspection));
            }
        }
        public ObservableCollection<CustomerVM> Customers { get; }
        public ObservableCollection<EventVM> Events { get; set; }
        public int SelectedIndex { get; set; }
        public ObservableCollection<StatusVM> Statuses { get; set; }
        public ObservableCollection<InspectorVM> ChosenInspectors { get; set; }
        public string Title { get; set; }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        public RelayCommand<Window> CloseWindowCommand { get; set; }
        public RelayCommand<Window> AddInspectionCommand { get; set; }
        public RelayCommand CanExecuteChangedCommand { get; set; }
        public RelayCommand PickInspectorsCommand { get; set; }

        public CreateEditInspectionViewModel()
        {
            Messenger.Default.Register<bool>(this, "UpdateAvailableList", c => RaisePropertyChanged(nameof(Inspection)));

            _context = new CustomFSContext();
            Customers = _context.CustomerCrud.GetAllCustomers();
            Events = _context.EventCrud.GetAllEvents();
            Statuses = _context.StatusCrud.GetAllStatusVMs();

            CloseWindowCommand = new RelayCommand<Window>(CloseWindow);
            AddInspectionCommand = new RelayCommand<Window>(AddInspection);
            CanExecuteChangedCommand = new RelayCommand(CanExecuteChanged);
            PickInspectorsCommand = new RelayCommand(OpenAvailableInspector);
        }

        public void SetInspection(int inspectionId)
        {
            if (inspectionId == -1)
            {
                Inspection = new InspectionVM(new Inspection())
                {
                    InspectionDate = new InspectionDateVM(new InspectionDate()
                    {
                        StartDate = DateTime.Now.Date,
                        EndDate = DateTime.Now.Date
                    }),
                    Event = new EventVM(new Event()
                    {
                        Customer = new Customer()
                    }),
                    Status = new StatusVM(new Status())
                };
                _context.Inspections.Add(Inspection.ToModel());
                Title = "Inspectie aanmaken";
            }
            else
            {
                Inspection = _context.InspectionCrud.GetInspectionById(inspectionId);
                Title = "Inspectie wijzigen";
            }
            SelectedIndex = GetIndex(Inspection.Event, Events);
            RaisePropertyChanged(nameof(SelectedIndex));
            RaisePropertyChanged(nameof(Inspection));
            RaisePropertyChanged(nameof(Title));
        }

        private int GetIndex(EventVM Obj, ObservableCollection<EventVM> List)
        {
            for (int i = 0; i < List.Count; i++)
                if (List[i].Id == Obj.Id)
                    return i;
            return -1;
        }

        private void CloseAction(Window window)
        {
            _context.Dispose();
            window.Close();
        }

        private void CloseWindow(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Scherm sluiten?", "Scherm sluiten", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _context.Dispose();
                Inspection = null;
                window.Close();
            }
        }

        public void AddInspection(Window window)
        {
            if (IsInternetConnected())
            {
                if (InspectionIsValid())
                {
                    var inspection = Inspection;
                    _context.SaveChanges();
                    Messenger.Default.Send(true, "UpdateInspectionList");
                    CloseAction(window);
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        public void OpenAvailableInspector()
        {
            if (IsInternetConnected())
            {
                new AvailableInspectorView(_context, Inspection).Show();
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        private void CanExecuteChanged()
        {
            AddInspectionCommand.RaiseCanExecuteChanged();
        }

        private bool InspectionIsValid()
        {

            if (Inspection.InspectionDate.StartDate == null)
            {
                MessageBox.Show("Een inspectie moet een startdatum hebben.");
                return false;
            }

            if (Inspection.InspectionDate.EndDate < Inspection.InspectionDate.StartDate)
            {
                MessageBox.Show("De einddatum mag niet voor de begindatum liggen.");
                return false;
            }

            if (Inspection.InspectionDate.StartDate != null && Inspection.InspectionDate.StartDate < DateTime.Now.Date)
            {
                MessageBox.Show("Een inspectie kan niet in het verleden worden gepland.");
                return false;
            }

            if (Inspection.InspectionDate.StartTime != null)
            {
                var regex = new Regex(@"^([0-9]|1[0-9]|2[0-3])(:[0-5][0-9]|:[0-9]){0,2}$");
                if (!regex.IsMatch(Inspection.InspectionDate.StartTime.ToString()))
                { 
                    MessageBox.Show("De begintijd is niet goed.");
                    return false;
                }
            }

            if (Inspection.InspectionDate.EndTime != null)
            {
                var regex = new Regex(@"^([0-9]|1[0-9]|2[0-3])(:[0-5][0-9]|:[0-9]){0,2}$");
                if (!regex.IsMatch(Inspection.InspectionDate.EndTime.ToString()))
                {
                    MessageBox.Show("De eindtijd is niet goed.");
                    return false;
                }
            }

            if (Inspection.InspectionDate.StartDate == Inspection.InspectionDate.EndDate)
            {
                if (Inspection.InspectionDate.StartTime > Inspection.InspectionDate.EndTime)
                {
                    MessageBox.Show("De eindtijd van een inspectie kan niet eerder dan de begintijd van de inspectie zijn.");
                    return false;
                }
            }

            if (Inspection.Name == null)
            {
                MessageBox.Show("Een inspectie moet een naam hebben.");
                return false;
            }

            if (Inspection.Name.Trim() == string.Empty)
            {
                MessageBox.Show("Een inspectie moet een naam hebben.");
                return false;
            }

            if (Inspection.Status.StatusName == null)
            {
                MessageBox.Show("Een inspectie moet een status hebben.");
                return false;
            }

            if (Inspection.Event.Zipcode == null)
            {
                MessageBox.Show("Een inspectie moet een evenement hebben.");
                return false;
            }

            return true;

        }
    }
}
