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

        private InspectorVM _selectedAvailaibleInspector { get; set; }

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

        public InspectorVM SelectedAvailaibleInspector
        {
            get { return _selectedAvailaibleInspector; }
            set
            {
                _selectedAvailaibleInspector = value;
                base.RaisePropertyChanged(nameof(SelectedAvailaibleInspector));
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

        internal void Init()
        {
            _customFSContext = new CustomFSContext();
        }

        public void SetInspection(int inspectionId)
        {
            _selectedInspection = _customFSContext.InspectionCrud.GetInspectionById(inspectionId);
            AvailableInspectors = _customFSContext.InspectorCrud.GetAllInspectorsFilteredByAvailability(
                new List<DateTime>{
                    _selectedInspection.InspectionDate.StartDate,
                    _selectedInspection.InspectionDate.EndDate
                });
            RaisePropertyChanged(nameof(AvailableInspectors));
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
                ChosenInspectors.Remove(inspectorChosen);
                AvailableInspectors.Add(inspectorChosen);
            } else
            {
                MessageBox.Show("Geen inspecteur geselecteerd.");
            }
        }

        private void SaveChanges(Window window)
        {
            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Wijzigingen opslaan??", "Opslaan wijzigingen", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    _customFSContext.SaveChanges();
                    window.Close();

                    // Update
                    Messenger.Default.Send(ChosenInspectors, "UpdateAvailableList");
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
