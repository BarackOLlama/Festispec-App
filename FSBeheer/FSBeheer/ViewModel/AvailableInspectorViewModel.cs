using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        private InspectorVM _selectedAvailaibleInspector { get; set; }

        private InspectorVM _selectedChosenInspector { get; set; }

        private InspectionVM _selectedInspection { get; set; }

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
            CustomFSContext = new CustomFSContext();
        }

        public void SetInspection(int inspectionId)
        {
            _selectedInspection = CustomFSContext.InspectionCrud.GetInspectionById(inspectionId);
            AvailableInspectors = CustomFSContext.InspectorCrud.GetAllInspectorsFilteredByAvailability(
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
                MessageBox.Show("No inspector selected");
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
                MessageBox.Show("No inspector selected");
            }
        }

        private void SaveChanges(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Save changes?", "Confirm action", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                // TODO:
                // Add Chosen Inspectors to Observable collection of meesturen via messenger en zelf laten afhandelen
                CustomFSContext.SaveChanges();
                window.Close();

                // Update to previous 
                Messenger.Default.Send(true, "UpdateAvailableList");
            }
        }

        private void Discard(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Close without saving?", "Confirm discard", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                CustomFSContext.Dispose();
                ChosenInspectors = null;
                window.Close();
            }
        }
    }
}
