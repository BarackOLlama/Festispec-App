using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.ViewModel
{
    public class AvailableInspectorViewModel : ViewModelBase
    {
        private CustomFSContext CustomFSContext;

        public ObservableCollection<AvailabilityVM> AvailableInspectors { get; set; }

        public ObservableCollection<AvailabilityVM> ChosenInspectors { get; set; }

        public RelayCommand<InspectorVM> SetInspectorCommand { get; set; }

        public RelayCommand<InspectorVM> RemoveInspectorCommand { get; set; }

        private InspectorVM _selectedInspector { get; set; }

        public AvailableInspectorViewModel()
        {
            Init();
            SetInspectorCommand = new RelayCommand<InspectorVM>(DummyMethod);
            RemoveInspectorCommand = new RelayCommand<InspectorVM>(DummyMethod);
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

        private void DummyMethod(InspectorVM commandParameter)
        {
            // iets
        }
    }
}
