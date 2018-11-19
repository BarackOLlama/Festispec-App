using FSBeheer.VM;
using FSBeheer.Model;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace FSBeheer.ViewModel
{
    public class InspectionManagementViewModel : ViewModelBase
    {
        private CustomFSContext _Context;
        public ObservableCollection<InspectionVM> Inspections { get; }

        public RelayCommand AddNewInspectionCommand;

        public InspectionManagementViewModel()
        {            
            _Context = new CustomFSContext();
            Inspections = _Context.InspectionCrud.GetInspections();

            AddNewInspectionCommand = new RelayCommand(AddNewInspection);
        }

        private void AddNewInspection()
        {

        }
    }
}
