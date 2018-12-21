using FSBeheer.Model;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class AvailableInspectorViewModel : ViewModelBase
    {
        private CustomFSContext CustomFSContext;

        public ObservableCollection<InspectorVM> AvailableInspectors { get; set; }

        public ObservableCollection<InspectorVM> ChosenInspectors { get; set; }

        public RelayCommand<InspectorVM> SetInspectorCommand { get; set; }

        public RelayCommand<InspectorVM> RemoveInspectorCommand { get; set; }

        public RelayCommand<Window> SaveChangesCommand { get; set; }

        public RelayCommand<Window> DiscardChangesCommand { get; set; }

        private InspectorVM _selectedAvailableInspector { get; set; }

        private InspectorVM _selectedChosenInspector { get; set; }

        private InspectionVM _selectedInspection { get; set; }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        public AvailableInspectorViewModel()
        {
            Init();

            ChosenInspectors = new ObservableCollection<InspectorVM>();

            SetInspectorCommand = new RelayCommand<InspectorVM>(AddInspector);
            RemoveInspectorCommand = new RelayCommand<InspectorVM>(RemoveInspector);
            SaveChangesCommand = new RelayCommand<Window>(SaveChanges);
            DiscardChangesCommand = new RelayCommand<Window>(Discard);
        }

        public InspectorVM SelectedAvailableInspector
        {
            get { return _selectedAvailableInspector; }
            set
            {
                _selectedAvailableInspector = value;
                base.RaisePropertyChanged(nameof(SelectedAvailableInspector));
            }
        }

        public InspectorVM SelectedChosenInspector
        {
            get { return _selectedChosenInspector; }
            set
            {
                _selectedChosenInspector = value;
                base.RaisePropertyChanged(nameof(SelectedChosenInspector));
            }
        }

        public ObservableCollection<InspectorVM> RemovedInspectors { get; set; }

        internal void Init()
        {
            CustomFSContext = new CustomFSContext();
        }

        public void SetContextInspectionId(CustomFSContext context, int inspectionId)
        {
            CustomFSContext = context;
            _selectedInspection = CustomFSContext.InspectionCrud.GetInspectionById(inspectionId);
            AvailableInspectors = CustomFSContext.InspectorCrud.GetAllInspectorsFilteredByAvailability(
            new List<DateTime>{
                    _selectedInspection.InspectionDate.StartDate,
                    _selectedInspection.InspectionDate.EndDate
            });
            ChosenInspectors = CustomFSContext.InspectorCrud.GetInspectorsByInspectionId(inspectionId);
            RemovedInspectors = new ObservableCollection<InspectorVM>();
            RaisePropertyChanged(nameof(AvailableInspectors));
            RaisePropertyChanged(nameof(ChosenInspectors));
        }

        private void AddInspector(InspectorVM inspectorAvailable)
        {
            if (inspectorAvailable != null)
            {
                ChosenInspectors.Add(inspectorAvailable);
                AvailableInspectors.Remove(inspectorAvailable);
            } else
            {
                MessageBox.Show("Er zijn geen inspecteurs geselecteerd.");
            }
        }

        private void RemoveInspector(InspectorVM inspectorChosen)
        {
            if (inspectorChosen != null)
            {
                if (_selectedInspection.Inspectors.Contains(inspectorChosen))
                {
                    RemovedInspectors.Add(inspectorChosen);
                }
                foreach (InspectorVM inspectorVM in _selectedInspection.Inspectors)
                {
                    if (inspectorVM.Id == inspectorChosen.Id)
                        RemovedInspectors.Add(inspectorChosen);
                }
                ChosenInspectors.Remove(inspectorChosen);
                AvailableInspectors.Add(inspectorChosen);
            } else
            {
                MessageBox.Show("Er zijn geen inspecteurs geselecteerd.");
            }
        }

        private void SaveChanges(Window window)
        {
            // moet nog gefixt worden

            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Wilt u de veranderingen opslaan?", "Bevestigen", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    _selectedInspection.Inspectors = ChosenInspectors;
                    foreach (InspectorVM inspectorVM in ChosenInspectors)
                    {
                        for (var start = _selectedInspection.InspectionDate.StartDate; start <= _selectedInspection.InspectionDate.EndDate; start = start.AddDays(1))
                        {
                            AvailabilityVM availabilityVM = new AvailabilityVM(new Availability())
                            {
                                Inspector = inspectorVM.ToModel(),
                                Scheduled = true,
                                Date = (DateTime?)start,
                                ScheduleStartTime = _selectedInspection.InspectionDate.StartTime,
                                ScheduleEndTime = _selectedInspection.InspectionDate.EndTime
                            };
                            CustomFSContext.Availabilities.Add(availabilityVM.ToModel());
                        }
                    }

                    
                    CustomFSContext.AvailabilityCrud.RemoveAvailabilitiesByInspectorList(RemovedInspectors, _selectedInspection);
                    

                    window.Close();
                    Messenger.Default.Send(true, "UpdateAvailableList");
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        private void Discard(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Wilt u sluiten zonder op te slaan?", "Confirm discard", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                CustomFSContext.Dispose();
                ChosenInspectors = null;
                window.Close();
            }
        }
    }
}
