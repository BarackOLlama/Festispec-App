﻿using FSBeheer.Model;
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

        private InspectorVM _selectedAvailableInspector { get; set; }

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

        public InspectorVM SelectedAvailableInspector
        {
            get { return _selectedAvailableInspector; }
            set
            {
                _selectedAvailableInspector = value;
                base.RaisePropertyChanged(nameof(SelectedAvailableInspector));
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

        public ObservableCollection<InspectorVM> RemovedInspectors { get; set; }

        internal void Init()
        {
            CustomFSContext = new CustomFSContext();
        }

        public void SetContextInspectionId(CustomFSContext context, int inspectionId)
        {
            CustomFSContext = context;
            _selectedInspection = CustomFSContext.InspectionCrud.GetInspectionById(inspectionId);
            AvailableInspectors = CustomFSContext.InspectorCrud.GetAllInspectorsFilteredByAvailability(
            new List<DateTime>{
                    _selectedInspection.InspectionDate.StartDate,
                    _selectedInspection.InspectionDate.EndDate
            });
            ChosenInspectors = CustomFSContext.InspectorCrud.GetInspectorsByInspectionId(inspectionId);
            RaisePropertyChanged(nameof(AvailableInspectors));
            RaisePropertyChanged(nameof(ChosenInspectors));
        }

        private void AddInspector(InspectorVM inspectorAvailable)
        {
            if (inspectorAvailable != null)
            {
                ChosenInspectors.Add(inspectorAvailable);
                AvailableInspectors.Remove(inspectorAvailable);
            } else
            {
                MessageBox.Show("Er zijn geen inspecteurs geselecteerd.");
            }
        }

        private void RemoveInspector(InspectorVM inspectorChosen)
        {
            if (inspectorChosen != null)
            {
                if (_selectedInspection.Inspectors.Contains(inspectorChosen))
                {
                    RemovedInspectors.Add(inspectorChosen);
                }
                ChosenInspectors.Remove(inspectorChosen);
                AvailableInspectors.Add(inspectorChosen);
            } else
            {
                MessageBox.Show("Er zijn geen inspecteurs geselecteerd.");
            }
        }

        private void SaveChanges(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Wilt u de veranderingen opslaan?", "Bevestigen", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _selectedInspection.Inspectors = ChosenInspectors;
                foreach (InspectorVM inspectorVM in ChosenInspectors)
                {
                    for (var start = _selectedInspection.InspectionDate.StartDate; start <= _selectedInspection.InspectionDate.EndDate; start = start.AddDays(1))
                    {
                        AvailabilityVM availabilityVM = new AvailabilityVM(new Availability());
                        availabilityVM.Inspector = inspectorVM.ToModel();
                        availabilityVM.Scheduled = true;
                        availabilityVM.Date = (DateTime?)start;
                        availabilityVM.ScheduleStartTime = _selectedInspection.InspectionDate.StartTime;
                        availabilityVM.ScheduleEndTime = _selectedInspection.InspectionDate.EndTime;
                        CustomFSContext.Availabilities.Add(availabilityVM.ToModel());
                    }
                }

                foreach (InspectorVM inspectorVM in RemovedInspectors)
                {

                }
                window.Close();

                // Update
                Messenger.Default.Send(true, "UpdateAvailableList");
            }
        }

        private void Discard(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Wilt u sluiten zonder op te slaan?", "Confirm discard", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                CustomFSContext.Dispose();
                ChosenInspectors = null;
                window.Close();
            }
        }
    }
}
