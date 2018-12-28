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
        private CustomFSContext _Context;

        public InspectionVM Inspection { get; set; }
        public ObservableCollection<CustomerVM> Customers { get; }
        public ObservableCollection<EventVM> Events { get; set; }
        public int SelectedIndex { get; set; }
        public ObservableCollection<StatusVM> Statuses { get; set; }

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
                CanExecuteChanged();
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
                CanExecuteChanged();
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
                CanExecuteChanged();
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
                CanExecuteChanged();
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
                    Inspection.Event = value;
                }
                RaisePropertyChanged(nameof(SelectedEvent));
                CanExecuteChanged();
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
                    Inspection.Status = value;
                }
                RaisePropertyChanged(nameof(SelectedStatus));
                CanExecuteChanged();
            }
        }

        public RelayCommand<Window> CancelInspectionCommand { get; set; }
        public RelayCommand<Window> AddInspectionCommand { get; set; }
        public RelayCommand CanExecuteChangedCommand { get; set; }
        public RelayCommand PickInspectorsCommand { get; set; }

        public CreateEditInspectionViewModel()
        {
            // Darjush laten weten
            Messenger.Default.Register<ObservableCollection<InspectorVM>>(this, "UpdateAvailableList", cl => SetInspectors(cl));

            _Context = new CustomFSContext();
            Customers = _Context.CustomerCrud.GetAllCustomers();
            Events = _Context.EventCrud.GetAllEvents();
            Statuses = _Context.StatusCrud.GetAllStatusVMs();

            CancelInspectionCommand = new RelayCommand<Window>(CancelInspection);
            AddInspectionCommand = new RelayCommand<Window>(AddInspection);
            CanExecuteChangedCommand = new RelayCommand(CanExecuteChanged);
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
                    InspectionDate = new InspectionDateVM(new InspectionDate())
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
                SelectedStatus = _Context.StatusCrud.GetStatusById(Inspection.Status.Id);
                StartDate = Inspection.InspectionDate.StartDate;
                EndDate = Inspection.InspectionDate.EndDate;
                StartTime = Inspection.InspectionDate.StartTime;
                EndTime = Inspection.InspectionDate.EndTime;
                RaisePropertyChanged(nameof(Inspection));
            }
            Events = _Context.EventCrud.GetAllEvents();
            SelectedIndex = GetIndex(Inspection.Event, Events);
            RaisePropertyChanged(nameof(Events));
            RaisePropertyChanged(nameof(SelectedIndex));
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
           _Context.Dispose();
           window.Close();
       }

       private void CancelInspection(Window window)
       {
           MessageBoxResult result = MessageBox.Show("Weet u zeker dat u deze inspectie wilt annuleren?", "Bevestig annulering inspectie", MessageBoxButton.OKCancel);
           if (result == MessageBoxResult.OK)
           {
               CloseAction(window);
           }
            CanExecuteChanged();
       }

        public void AddInspection(Window window)
        {
            if (IsInternetConnected())
            {
                // Inspectie aanmaken in de database met alle velden die ingevuld zijn
                _Context.InspectionCrud.GetAllInspections().Add(Inspection);
                _Context.SaveChanges();

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

       private void CanExecuteChanged()
       {
           AddInspectionCommand.RaiseCanExecuteChanged();
       }



    }
}
