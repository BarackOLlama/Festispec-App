using FSBeheer.Crud;
using FSBeheer.Model;
using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class CreateEditInspectionViewModel : ViewModelBase
    {
        private CustomFSContext _Context;

        public InspectionVM Inspection { get; set; }
        public ObservableCollection<CustomerVM> Customers { get; }
        public ObservableCollection<EventVM> Events { get; set; }
        public ObservableCollection<StatusVM> Statuses { get; set; }
        public ObservableCollection<InspectorVM> ChosenInspectors { get; set; }
        public string Title { get; set; }
        private List<InspectorVM> _listOfInspectors;

        public string WarningText { get; set; }
        
        public RelayCommand<Window> CancelInspectionCommand { get; set; }
        public RelayCommand<Window> AddInspectionCommand { get; set; }
        public RelayCommand CanExecuteChangedCommand { get; set; }
        public RelayCommand PickInspectorsCommand { get; set; }

        public CreateEditInspectionViewModel()
        {
            // Darjush laten weten
            Messenger.Default.Register<ObservableCollection<InspectorVM>>(this, "UpdateAvailableList", cl => SetInspectors(cl));

            _Context = new CustomFSContext();
            Customers = _Context.CustomerCrud.GetAllCustomers();
            Events = _Context.EventCrud.GetAllEvents();
            Statuses = _Context.StatusCrud.GetAllStatusVMs();
            _listOfInspectors = new List<InspectorVM>();

            CancelInspectionCommand = new RelayCommand<Window>(CancelInspection);
            AddInspectionCommand = new RelayCommand<Window>(AddInspection);
            CanExecuteChangedCommand = new RelayCommand(CanExecuteChanged);
            PickInspectorsCommand = new RelayCommand(OpenAvailable);
        }

        public void SetInspectors(ObservableCollection<InspectorVM> SelectedInspectors)
        {
            // TODO: Not in memory/database after reopening the screen
            _listOfInspectors = new InspectorCrud(_Context).GetInspectorsByInspectionId(Inspection.Id);

            ChosenInspectors = _Context.InspectorCrud.GetInspectorsByList(SelectedInspectors);
            if (ChosenInspectors != null && ChosenInspectors.Count != 0)
            {
                foreach (InspectorVM inspectorVM in ChosenInspectors)
                {
                    _listOfInspectors.Add(inspectorVM);

                    for (var start = Inspection.InspectionDate.StartDate; start <= Inspection.InspectionDate.EndDate; start = start.AddDays(1))
                    {
                        AvailabilityVM availabilityVM = new AvailabilityVM(new Availability());
                        availabilityVM.Inspector = inspectorVM.ToModel();
                        availabilityVM.Scheduled = true;
                        availabilityVM.Date = (DateTime?)start;
                        availabilityVM.ScheduleStartTime = Inspection.InspectionDate.StartTime;
                        availabilityVM.ScheduleEndTime = Inspection.InspectionDate.EndTime;
                        _Context.Availabilities.Add(availabilityVM.ToModel());
                    }
                }
            }
            Inspection.Inspectors = new ObservableCollection<InspectorVM>(_listOfInspectors);
            _Context.SaveChanges();
            RaisePropertyChanged(nameof(Inspection));
        }

        public void SetInspection(int inspectionId)
        {
            if (inspectionId == -1)
            {
                Inspection = new InspectionVM(new Inspection())
                {
                    InspectionDate = new InspectionDateVM(new InspectionDate())
                };
                _Context.Inspections.Add(Inspection.ToModel());
                Title = "Inspectie aanmaken";
            }
            else
            {
                Inspection = _Context.InspectionCrud.GetInspectionById(inspectionId);
                Title = "Inspectie wijzigen";
            }

            RaisePropertyChanged(nameof(Inspection));
        }

        private void CloseAction(Window window)
        {
            _Context.Dispose();
            window.Close();
        }

        private void CancelInspection(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Weet u zeker dat u deze inspectie wilt annuleren?", "Bevestig annulering inspectie", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                CloseAction(window);
            }
            CanExecuteChanged();
        }

        public void AddInspection(Window window)
        {            
            //if (ChosenInspectors != null && ChosenInspectors.Count != 0)
            //{
            //    foreach (InspectorVM inspectorVM in ChosenInspectors)
            //    {
            //        for (var start = Inspection.InspectionDate.StartDate; start <= Inspection.InspectionDate.EndDate; start = start.AddDays(1))
            //        {
            //            AvailabilityVM availabilityVM = new AvailabilityVM(new Availability());
            //            availabilityVM.Inspector = inspectorVM.ToModel();
            //            availabilityVM.Scheduled = true;
            //            availabilityVM.Date = (DateTime?) start;
            //            availabilityVM.ScheduleStartTime = Inspection.InspectionDate.StartTime;
            //            availabilityVM.ScheduleEndTime = Inspection.InspectionDate.EndTime;
            //            _Context.Availabilities.Add(availabilityVM.ToModel());
            //        }
            //    }
            //}

            if (Inspection.Status.StatusName == "Afgerond")
            {

            }

            if (CheckStartDateValidity())
            {
                _Context.SaveChanges();

                Messenger.Default.Send(true, "UpdateInspectionList");
                //CloseAction(window);
            }
            else
            {
                MessageBox.Show("De ingevulde datums zijn niet correct. Voer correcte begin- en einddatums in en probeer het opnieuw.");
                return;
            }            
        }

        public void OpenAvailable()
        {
            if (CheckStartDateValidity())
            {
                new AvailableInspectorView(Inspection.Id).Show();
            }
            else
            {
                MessageBox.Show("De ingevulde datums zijn niet correct. Voer correcte begin- en einddatums in en probeer het opnieuw.");
            }
        }

        private void CanExecuteChanged()
        {
            AddInspectionCommand.RaiseCanExecuteChanged();
        }

        private bool CheckSaveAllowed(Window window)
        {
            if (Inspection != null)
            {
                if (string.IsNullOrEmpty(Inspection.Name))
                {
                    WarningText = "Het veld Naam mag niet leeg zijn";
                    RaisePropertyChanged(nameof(WarningText));
                    return false;
                }
                else if (Inspection.Inspectors == null)
                {
                    WarningText = "Het veld Inspecteur(s) mag niet leeg zijn";
                    RaisePropertyChanged(nameof(WarningText));
                    return false;
                }
                else
                {
                    WarningText = "";
                    RaisePropertyChanged(nameof(WarningText));
                    return true;
                }
            }
            else
            {
                return false;
            }                
        }

        private bool CheckStartDateValidity()
        {
            if (Inspection.InspectionDate.StartDate >= new DateTime(1753, 1, 1))
            {
                return true;
            }
            return false;
        }
    }
}
