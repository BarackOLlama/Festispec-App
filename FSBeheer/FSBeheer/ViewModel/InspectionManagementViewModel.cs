using FSBeheer.VM;
using FSBeheer.Model;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace FSBeheer.ViewModel
{
    public class InspectionManagementViewModel : ViewModelBase
    {
        private CustomFSContext _Context;
        public ObservableCollection<InspectionVM> Inspections { get; }

        public InspectionManagementViewModel()
        {            
            _Context = new CustomFSContext();
            Inspections = _Context.InspectionCrud.GetInspections();
        }
    }
}
