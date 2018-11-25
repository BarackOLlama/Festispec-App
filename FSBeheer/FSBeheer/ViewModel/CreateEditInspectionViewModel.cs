using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;

namespace FSBeheer.ViewModel
{
    public class CreateEditInspectionViewModel : ViewModelBase
    {
        private CustomFSContext _Context;
        private InspectionManagementViewModel _InspectionManagementViewModel;

        public ObservableCollection<CustomerVM> Customers { get; }
        public ObservableCollection<EventVM> Events { get; }
        public ObservableCollection<StatusVM> Statuses { get; }
        
        public DateTime? NewStartDate { get; set; }
        public DateTime? NewEndDate { get; set; }
        public TimeSpan? NewStartTime { get; set; }
        public TimeSpan? NewEndTime { get; set; }
        public InspectionVM SelectedInspection { get; set; }
        public EventVM SelectedEvent { get; set; }
        public RelayCommand CancelInspectionCommand { get; set; }
        public RelayCommand AddInspectionCommand { get; set; }

        public CreateEditInspectionViewModel(InspectionManagementViewModel inspectionManagementViewModel)
        {
            _Context = new CustomFSContext();
            Customers = _Context.CustomerCrud.GetAllCustomerVMs();
            Events = _Context.EventCrud.GetAllEventVMs();
            CancelInspectionCommand = new RelayCommand(CancelInspection);
            AddInspectionCommand = new RelayCommand(AddInspection);
            _InspectionManagementViewModel = inspectionManagementViewModel;
            if (_InspectionManagementViewModel.SelectedInspection != null)
            {
                SelectedInspection = _InspectionManagementViewModel.SelectedInspection;
            };
        }

        public void AddInspection()
        {
            // Inspectie aanmaken in de database met alle velden die ingevuld zijn
        }

        public void CancelInspection()
        {
            // CreateInspectionView sluiten en veranderingen ongedaan maken
        }
    }
}
