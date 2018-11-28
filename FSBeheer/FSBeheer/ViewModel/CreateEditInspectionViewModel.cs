﻿using FSBeheer.Model;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Linq;

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
        public TimeSpan? NewStartTime {
            get;
            set; }
        public TimeSpan? NewEndTime { get; set; }
        public EventVM SelectedEvent { get; set; }
        public StatusVM SelectedStatus {
            get;
            set; }
        public InspectionVM Inspection { get; set; }
        public RelayCommand CancelInspectionCommand { get; set; }
        public RelayCommand AddInspectionCommand { get; set; }

        public CreateEditInspectionViewModel()
        {
            _Context = new CustomFSContext();
            Customers = _Context.CustomerCrud.GetAllCustomerVMs();

            Events = _Context.EventCrud.GetAllEventVMs();
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
        }

        public void SetInspection(InspectionVM inspection)
        {
            if (inspection == null)
            {
                Inspection = new InspectionVM(new Inspection());
                _Context.Inspections.Add(Inspection.ToModel());
                RaisePropertyChanged(nameof(Inspection));
            }
            else
            {
                Inspection = new InspectionVM(_Context.Inspections
                    .FirstOrDefault(i => i.Id == inspection.Id));
                RaisePropertyChanged(nameof(Inspection));
            }
        }

        public void AddInspection()
        {
            // Inspectie aanmaken in de database met alle velden die ingevuld zijn
            _Context.InspectionCrud.GetAllInspectionVMs().Add(Inspection);
            _Context.SaveChanges();

            Messenger.Default.Send(true, "UpdateInspectionList");
        }

        public void CancelInspection()
        {
            // CreateInspectionView sluiten en veranderingen ongedaan maken
        }
    }
}
