using FSBeheer.Model;
using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
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

        public RelayCommand<Window> CancelInspectionCommand { get; set; }
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

            CancelInspectionCommand = new RelayCommand<Window>(CancelInspection);
            AddInspectionCommand = new RelayCommand<Window>(AddInspection);
            CanExecuteChangedCommand = new RelayCommand(CanExecuteChanged);
            PickInspectorsCommand = new RelayCommand(OpenAvailable);
        }

        public void SetInspection(int inspectionId)
        {
            if (inspectionId == -1)
            {
                Inspection = new InspectionVM(new Inspection())
                {
                    InspectionDate = new InspectionDateVM(new InspectionDate()),
                    Event = new EventVM(new Event()
                    {
                        Customer = new Customer()
                    })
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

        private void CancelInspection(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Weet u zeker dat u deze inspectie wilt annuleren?", "Bevestig annulering inspectie", MessageBoxButton.OKCancel);
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

        public void OpenAvailable()
        {
            if (IsInternetConnected())
            {
                if (InspectionIsValid())
                {
                    new AvailableInspectorView(_context, Inspection.Id).Show();
                }
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
            if (Inspection.InspectionDate.StartDate <= DateTime.Now)
            {
                MessageBox.Show("Een inspectie kan niet in het verleden worden gepland.");
                return false;
            }

            if (Inspection.InspectionDate.EndDate <= Inspection.InspectionDate.StartDate)
            {
                MessageBox.Show("De einddatum moet na de begindatum zijn.");
                return false;
            }

            if (Inspection.Inspectors == null)
            {
                MessageBox.Show("Een inspectie moet minstens een inspecteur hebben.");
                return false;
            }

            if (Inspection.Name.Trim() == string.Empty)
            {
                MessageBox.Show("Een inspectie moet een naam hebben.");
                return false;
            }

            return true;

        }
    }
}
