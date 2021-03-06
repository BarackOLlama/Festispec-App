﻿using FSBeheer.Model;
using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class CreateEditInspectionViewModel : ViewModelBase
    {
        private CustomFSContext _context;

        private InspectionVM _inspection;
        public InspectionVM Inspection
        {
            get { return _inspection; }
            set
            {
                _inspection = value;
                RaisePropertyChanged(nameof(Inspection));
            }
        }
        public bool CurrentlyEditingInspection { get; set; }

        public ObservableCollection<CustomerVM> Customers { get; }
        public ObservableCollection<EventVM> Events { get; set; }
        public int SelectedIndex { get; set; }
        public ObservableCollection<StatusVM> Statuses { get; set; }
        public string Title { get; set; }
        public ObservableCollection<InspectorVM> ChosenInspectors { get; set; }
        public ObservableCollection<InspectorVM> RemovedInspectors { get; set; }
        public ObservableCollection<InspectorVM> AvailableInspectors { get; set; }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        public RelayCommand<Window> CloseWindowCommand { get; set; }
        public RelayCommand<Window> SaveChangesCommand { get; set; }
        public RelayCommand CanExecuteChangedCommand { get; set; }
        public RelayCommand PickInspectorsCommand { get; set; }
        public RelayCommand<Window> DeleteInspectionCommand { get; set; }

        public CreateEditInspectionViewModel()
        {
            Messenger.Default.Register<ObservableCollection<InspectorVM>[]>(this, "UpdateInspectorList", c => UpdateInspectors(c));

            _context = new CustomFSContext();
            Customers = _context.CustomerCrud.GetAllCustomers();
            Events = _context.EventCrud.GetAllEvents();
            Statuses = _context.StatusCrud.GetAllStatusVMs();
            ChosenInspectors = new ObservableCollection<InspectorVM>();
            RemovedInspectors = new ObservableCollection<InspectorVM>();

            CloseWindowCommand = new RelayCommand<Window>(CloseWindow);
            SaveChangesCommand = new RelayCommand<Window>(SaveChanges);
            CanExecuteChangedCommand = new RelayCommand(CanExecuteChanged);
            PickInspectorsCommand = new RelayCommand(OpenAvailableInspector);
            DeleteInspectionCommand = new RelayCommand<Window>(DeleteInspection);
        }

        private void DeleteInspection(Window window)
        {
            var result = MessageBox.Show("Inspectie verwijderen?", "Inspectie verwijderen", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                RemoveScheduleItemsAndInspectorsOfInspection();
                Inspection.IsDeleted = true;
                _context.SaveChanges();
                _context.Events.RemoveRange(_context.Events.Where(e => e.Name == null));
                _context.Customers.RemoveRange(_context.Customers.Where(c => c.Name == null));
                _context.SaveChanges();
                Messenger.Default.Send(true, "UpdateInspectionList");
                window.Close();
            }
        }

        private void UpdateInspectors(ObservableCollection<InspectorVM>[] ChosenAndRemovedInspectors)
        {
            ChosenInspectors = ChosenAndRemovedInspectors[0];
            RemovedInspectors = ChosenAndRemovedInspectors[1];
            AvailableInspectors = ChosenAndRemovedInspectors[2];
            RaisePropertyChanged(nameof(Inspection));
        }

        private bool CheckIfScheduleItemExists(InspectorVM inspectorVM)
        {
            var test = _context.ScheduleItems.ToList();
            var scheduleitems = _context.ScheduleItems
                .ToList()
                .Where(s => s.IsDeleted == false)
                .Where(s => s.Inspector.Id == inspectorVM.Id)
                .Select(i => new ScheduleItemVM(i));
            var _scheduleitems = new ObservableCollection<ScheduleItemVM>(scheduleitems);
            foreach (ScheduleItemVM scheduleItem in _scheduleitems)
            {
                if (scheduleItem.Date >= Inspection.InspectionDate.StartDate && scheduleItem.Date <= Inspection.InspectionDate.EndDate)
                {
                    return true;
                }
            }
            return false;
        }

        public void SetInspection(int inspectionId)
        {
            if (inspectionId == -1)
            {
                CurrentlyEditingInspection = false;
                Inspection = new InspectionVM(new Inspection())
                {
                    InspectionDate = new InspectionDateVM(new InspectionDate()
                    {
                        StartDate = DateTime.Now.Date,
                        EndDate = DateTime.Now.Date
                    }),
                    Event = new EventVM(new Event()
                    {
                        Customer = new Customer()
                    }),
                    Status = new StatusVM(new Status())
                };
                _context.Inspections.Add(Inspection.ToModel());
                Title = "Inspectie aanmaken";
            }
            else
            {
                CurrentlyEditingInspection = true;
                Inspection = _context.InspectionCrud.GetInspectionById(inspectionId);
                ChosenInspectors = Inspection.Inspectors;
                Title = "Inspectie wijzigen";
            }
            SelectedIndex = GetIndex(Inspection.Event, Events);
            RaisePropertyChanged(nameof(SelectedIndex));
            RaisePropertyChanged(nameof(Inspection));
            RaisePropertyChanged(nameof(Title));
            base.RaisePropertyChanged(nameof(CurrentlyEditingInspection));
        }

        private int GetIndex(EventVM Obj, ObservableCollection<EventVM> List)
        {
            for (int i = 0; i < List.Count; i++)
                if (List[i].Id == Obj.Id)
                    return i;
            return -1;
        }

        private void CloseAction(Window window)
        {
            _context.Dispose();
            window.Close();
        }

        private void CloseWindow(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Scherm sluiten?", "Scherm sluiten", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _context.Dispose();
                Inspection = null;
                window.Close();
            }
        }

        public void SaveChanges(Window window)
        {
            if (IsInternetConnected())
            {
                if (InspectionIsValid())
                {
                    RemoveScheduleItemsAndInspectorsOfInspection();
                    foreach (InspectorVM inspectorVM in ChosenInspectors)
                    {
                        for (var start = Inspection.InspectionDate.StartDate; start <= Inspection.InspectionDate.EndDate; start = start.AddDays(1))
                        {
                            ScheduleItemVM scheduleItemVM = new ScheduleItemVM(new ScheduleItem())
                            {
                                Inspector = inspectorVM,
                                Scheduled = true,
                                Date = (DateTime?)start,
                                ScheduleStartTime = Inspection.InspectionDate.StartTime,
                                ScheduleEndTime = Inspection.InspectionDate.EndTime,
                                IsDeleted = false
                            };
                            _context.ScheduleItems.Add(scheduleItemVM.ToModel());
                        }
                        Inspection.Inspectors.Add(inspectorVM);
                    }

                    _context.SaveChanges();
                    _context.Events.RemoveRange(_context.Events.Where(e => e.Name == null));
                    _context.Customers.RemoveRange(_context.Customers.Where(c => c.Name == null));
                    _context.Statuses.RemoveRange(_context.Statuses.Where(s => s.StatusName == null));
                    _context.SaveChanges();
                    Messenger.Default.Send(true, "UpdateInspectionList");
                    MessageBox.Show("Inspection saved.");
                    CloseAction(window);
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        public void OpenAvailableInspector()
        {
            if (IsInternetConnected())
            {
                new AvailableInspectorView(_context, Inspection, AvailableInspectors).Show();
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        private void CanExecuteChanged()
        {
            SaveChangesCommand.RaiseCanExecuteChanged();
        }

        private bool InspectionIsValid()
        {

            if (Inspection.InspectionDate.StartDate == null)
            {
                MessageBox.Show("Een inspectie moet een startdatum hebben.");
                return false;
            }

            if (Inspection.InspectionDate.EndDate < Inspection.InspectionDate.StartDate)
            {
                MessageBox.Show("De einddatum mag niet voor de begindatum liggen.");
                return false;
            }

            if (Inspection.InspectionDate.StartTime != null)
            {
                var regex = new Regex(@"^([0-1][0-9]|2[0-3])(:[0-5][0-9]|:[0-9]){0,2}$");
                if (!regex.IsMatch(Inspection.InspectionDate.StartTime.ToString()))
                {
                    MessageBox.Show("De begintijd is niet goed.");
                    return false;
                }
            }

            if (Inspection.InspectionDate.EndTime != null)
            {
                var regex = new Regex(@"^([0-1][0-9]|2[0-3])(:[0-5][0-9]|:[0-9]){0,2}$");
                if (!regex.IsMatch(Inspection.InspectionDate.EndTime.ToString()))
                {
                    MessageBox.Show("De eindtijd is niet goed.");
                    return false;
                }
            }

            if (Inspection.InspectionDate.StartDate != null && Inspection.InspectionDate.StartDate == Inspection.InspectionDate.EndDate)
            {
                if (Inspection.InspectionDate.StartTime > Inspection.InspectionDate.EndTime)
                {
                    MessageBox.Show("De eindtijd van een inspectie kan niet eerder dan de begintijd van de inspectie zijn.");
                    return false;
                }
            }

            if (Inspection.Name == null)
            {
                MessageBox.Show("Een inspectie moet een naam hebben.");
                return false;
            }

            if (Inspection.Name.Trim() == string.Empty)
            {
                MessageBox.Show("Een inspectie moet een naam hebben.");
                return false;
            }

            if (Inspection.Status.StatusName == null)
            {
                MessageBox.Show("Een inspectie moet een status hebben.");
                return false;
            }

            if (Inspection.Event.Zipcode == null)
            {
                MessageBox.Show("Een inspectie moet een evenement hebben.");
                return false;
            }

            if (_context.QuestionnaireCrud.GetQuestionnaireByInspectionId(Inspection.Id) == null && Inspection.Status.StatusName == "Ingepland")
            {
                MessageBox.Show("De inspectie kan niet op \"Ingepland\" gezet worden omdat de inspectie geen vragenlijst heeft.");
                return false;
            }

            return true;

        }

        private void RemoveScheduleItemsAndInspectorsOfInspection()
        {
            var removeScheduleItems = new ObservableCollection<ScheduleItem>();
            var allScheduleItems = _context.ScheduleItems.ToList().Select(si => new ScheduleItemVM(si)).ToList();
            foreach (ScheduleItemVM scheduleItem in allScheduleItems)
            {
                if (scheduleItem.Inspector.Inspection.Count() > 0 &&
                    scheduleItem.Inspector.Inspection.ToList().Select(i => i.Id).First() == Inspection.Id)
                    removeScheduleItems.Add(scheduleItem.ToModel());
            }

            foreach (ScheduleItem scheduleItem in removeScheduleItems)
            {
                _context.ScheduleItems.Where(si => si.Id == scheduleItem.Id).First().IsDeleted = true;
            }

            foreach (InspectorVM inspector in ChosenInspectors)
            {
                Inspection.Inspectors.Remove(inspector);
            }
        }
    }
}
