using System.Collections.ObjectModel;
using FSBeheer.VM;
using GalaSoft.MvvmLight;

namespace FSBeheer.ViewModel
{

    public class ChooseInspectorViewModel : ViewModelBase
    {
        private CustomFSContext _Context;
        public ObservableCollection<InspectionVM> Inspections { get; }
        public ObservableCollection<AvailabilityVM> Availabilities { get; }
        public ObservableCollection<InspectionDateVM> InspectionDates { get; }

        public ChooseInspectorViewModel()
        {
            _Context = new CustomFSContext();
            Inspections = _Context.InspectionCrud.GetInspections();
            Availabilities = _Context.AvailabilityCrud.GetAvailabilities();
        }

    }
}
