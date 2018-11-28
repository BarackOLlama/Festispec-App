using FSBeheer.View;
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
        public ObservableCollection<EventVM> Events { get; set; }
        public ObservableCollection<string> EventNames { get; set; }
        public ObservableCollection<StatusVM> Statuses { get; set; }
        public ObservableCollection<string> StatusNames { get; set; }
        
        public DateTime? NewStartDate { get; set; }
        public DateTime? NewEndDate { get; set; }
        public TimeSpan? NewStartTime { get; set; }
        public TimeSpan? NewEndTime { get; set; }
        public InspectionVM SelectedInspection { get; set; }
        public EventVM SelectedEvent { get; set; }
        public StatusVM SelectedStatus { get; set; }
        public RelayCommand CancelInspectionCommand { get; set; }
        public RelayCommand AddInspectionCommand { get; set; }

        public RelayCommand PickInspectorsCommand { get; set; }

        public CreateEditInspectionViewModel(InspectionManagementViewModel inspectionManagementViewModel)
        {
            _Context = new CustomFSContext();
            Customers = _Context.CustomerCrud.GetAllCustomerVMs();

            Events = _Context.EventCrud.GetAllEvents();
            ObservableCollection<string> _EventList = new ObservableCollection<string>();
            foreach (EventVM eventVM in Events)
            {
                _EventList.Add(eventVM.Name);
            }
            EventNames = _EventList;

            Statuses = _Context.StatusCrud.GetAllStatusVMs();
            ObservableCollection<string> _StatusList = new ObservableCollection<string>();
            foreach (StatusVM statusVM in Statuses)
            {
                _StatusList.Add(statusVM.StatusName);
            }
            StatusNames = _StatusList;

            CancelInspectionCommand = new RelayCommand(CancelInspection);
            AddInspectionCommand = new RelayCommand(AddInspection);
            _InspectionManagementViewModel = inspectionManagementViewModel;
            if (_InspectionManagementViewModel.SelectedInspection != null)
            {
                SelectedInspection = _InspectionManagementViewModel.SelectedInspection;
            };

            PickInspectorsCommand = new RelayCommand(OpenAvailable);
        }

        public void AddInspection()
        {
            // Inspectie aanmaken in de database met alle velden die ingevuld zijn
        }

        public void CancelInspection()
        {
            // CreateInspectionView sluiten en veranderingen ongedaan maken
        }

        public void OpenAvailable()
        {
            new AvailableInspectorView(SelectedInspection.Id).Show();
        }
    }
}
