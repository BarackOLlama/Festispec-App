using FSBeheer.Crud;
using FSBeheer.Model;
using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
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
        public ObservableCollection<InspectorVM> ChosenInspectors { get; set; }
        public string Title { get; set; }
        private List<InspectorVM> _listOfInspectors;

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

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

        public string WarningText { get; set; }
        
        public RelayCommand<Window> CancelInspectionCommand { get; set; }
        public RelayCommand<Window> AddInspectionCommand { get; set; }
        public RelayCommand CanExecuteChangedCommand { get; set; }
        public RelayCommand PickInspectorsCommand { get; set; }

        public CreateEditInspectionViewModel()
        {
            // Darjush laten weten
            Messenger.Default.Register<bool>(this, "UpdateAvailableList", c => RaisePropertyChanged(nameof(Inspection)));

            _Context = new CustomFSContext();
            Customers = _Context.CustomerCrud.GetAllCustomers();
            Events = _Context.EventCrud.GetAllEvents();
            Statuses = _Context.StatusCrud.GetAllStatusVMs();

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
                    InspectionDate = new InspectionDateVM(new InspectionDate())
                };
                _Context.Inspections.Add(Inspection.ToModel());
                Title = "Inspectie aanmaken";
            }
            else
            {
                Inspection = _Context.InspectionCrud.GetInspectionById(inspectionId);
                Title = "Inspectie wijzigen";
            }

            RaisePropertyChanged(nameof(Inspection));
        }

        private void CloseAction(Window window)
        {
            _Context.Dispose();
            window.Close();
        }

        private void CancelInspection(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Weet u zeker dat u deze inspectie wilt annuleren?", "Bevestig annulering inspectie", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _Context.Dispose();
                Inspection = null;
                window.Close();
            }
        }

        public void AddInspection()
        {
            if (IsInternetConnected())
            {
                if (CheckStartDateValidity())
                {
                    // Inspectie aanmaken in de database met alle velden die ingevuld zijn
                    _Context.InspectionCrud.GetAllInspections().Add(Inspection);
                    _Context.SaveChanges();
                    Messenger.Default.Send(true, "UpdateInspectionList");

                }
                else
                {
                    MessageBox.Show("De ingevulde datums zijn niet correct. Voer correcte begin- en einddatums in en probeer het opnieuw.");
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
                new AvailableInspectorView(Inspection.Id).Show();
            else
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            if (CheckStartDateValidity())
            {
                new AvailableInspectorView(_Context, Inspection.Id).Show();
            }
            else
            {
                MessageBox.Show("De ingevulde datums zijn niet correct. Voer correcte begin- en einddatums in en probeer het opnieuw.");
            }
        }

        private void CanExecuteChanged()
        {
            AddInspectionCommand.RaiseCanExecuteChanged();
        }

        private bool CheckSaveAllowed(Window window)
        {
            if (Inspection != null)
            {
                if (string.IsNullOrEmpty(Inspection.Name))
                {
                    WarningText = "Het veld Naam mag niet leeg zijn";
                    RaisePropertyChanged(nameof(WarningText));
                    return false;
                }
                else if (Inspection.Inspectors == null)
                {
                    WarningText = "Het veld Inspecteur(s) mag niet leeg zijn";
                    RaisePropertyChanged(nameof(WarningText));
                    return false;
                }
                else
                {
                    WarningText = "";
                    RaisePropertyChanged(nameof(WarningText));
                    return true;
                }
            }
            else
            {
                return false;
            }                
        }

        private bool CheckStartDateValidity()
        {
            if (Inspection.InspectionDate.StartDate >= new DateTime(1753, 1, 1))
            {
                return true;
            }
            return false;
            
        }
    }
}
