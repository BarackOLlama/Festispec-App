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
        private CustomFSContext _customFSContext;

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
            _customFSContext = new CustomFSContext();
        }

        public void SetContextInspectionId(CustomFSContext context, int inspectionId)
        {
            _selectedInspection = _customFSContext.InspectionCrud.GetInspectionById(inspectionId);
            AvailableInspectors = _customFSContext.InspectorCrud.GetAllInspectorsFilteredByAvailability(
                new List<DateTime>{
                    _selectedInspection.InspectionDate.StartDate,
                    _selectedInspection.InspectionDate.EndDate
            });
            ChosenInspectors = _customFSContext.InspectorCrud.GetInspectorsByInspectionId(inspectionId);
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
                MessageBox.Show("Geen inspecteur geselecteerd.");
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
                MessageBox.Show("Geen inspecteur geselecteerd.");
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
                            _customFSContext.Availabilities.Add(availabilityVM.ToModel());
                        }
                    }

                    
                    _customFSContext.AvailabilityCrud.RemoveAvailabilitiesByInspectorList(RemovedInspectors, _selectedInspection);
                    

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
            MessageBoxResult result = MessageBox.Show("Sluiten zonder opslaan?", "Bevestiging annulering", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _customFSContext.Dispose();
                ChosenInspectors = null;
                window.Close();
            }
        }
    }
}
