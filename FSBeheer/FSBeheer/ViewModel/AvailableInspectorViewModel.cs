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

        public ObservableCollection<AvailabilityVM> AvailableInspectors { get; set; }

        public ObservableCollection<AvailabilityVM> ChosenInspectors { get; set; }

        public RelayCommand<InspectorVM> SetInspectorCommand { get; set; }

        public RelayCommand<InspectorVM> RemoveInspectorCommand { get; set; }

        public RelayCommand SaveChangesCommand { get; set; }

        public RelayCommand<Window> DiscardChangesCommand { get; set; }

        private InspectorVM _selectedInspector { get; set; }

        public AvailableInspectorViewModel()
        {
            Init();
            SetInspectorCommand = new RelayCommand<InspectorVM>(AddInspector);
            RemoveInspectorCommand = new RelayCommand<InspectorVM>(DummyMethod);
            SaveChangesCommand = new RelayCommand(SaveChanges);
            DiscardChangesCommand = new RelayCommand<Window>(Discard);
        }

        public InspectorVM SelectedInspector
        {
            get { return _selectedInspector; }
            set
            {
                _selectedInspector = value;
                base.RaisePropertyChanged(nameof(SelectedInspector));
            }
        }

        internal void Init()
        {
            CustomFSContext = new CustomFSContext();
            AvailableInspectors = CustomFSContext.AvailabilityCrud.GetAvailabilities();
            RaisePropertyChanged(nameof(AvailableInspectors));
        }

        private void AddInspector(InspectorVM commandParameter)
        {

        }

        private void DummyMethod(InspectorVM commandParameter)
        {
            // iets
        }

        private void SaveChanges()
        {
            MessageBoxResult result = MessageBox.Show("Save changes?", "Confirm action", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                // CustomFSContext.AvailabilityCrud.GetAvailabilities().Add(Inspector);
                CustomFSContext.SaveChanges();

                Messenger.Default.Send(true, "UpdateAvailableList"); 
            }
        }

        private void Discard(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Close without saving?", "Confirm discard", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                CustomFSContext.Dispose();
                // Inspector = null;
                window.Close();
            }
        }
    }
}
