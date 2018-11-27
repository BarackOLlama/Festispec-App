using FSBeheer.VM;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.ViewModel
{
    public class AvailableInspectorViewModel
    {
        public ObservableCollection<AvailabilityVM> AvailableInspectors;

        public ObservableCollection<AvailabilityVM> ChosenInspectors;

        public RelayCommand<InspectorVM> ChooseInspectorCommand;

        public AvailableInspectorViewModel()
        {
            ChooseInspectorCommand = new RelayCommand<InspectorVM>(DummyMethod);
        }

        private void DummyMethod(InspectorVM commandParameter)
        {
            // iets
        }
    }
}
