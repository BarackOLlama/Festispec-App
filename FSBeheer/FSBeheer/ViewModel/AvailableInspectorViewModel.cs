﻿using FSBeheer.Model;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class AvailableInspectorViewModel : ViewModelBase
    {
        private CustomFSContext _context;

        public ObservableCollection<InspectorVM> AvailableInspectors { get; set; }
        public ObservableCollection<InspectorVM> ChosenInspectors { get; set; }
        public ObservableCollection<InspectorVM> RemovedInspectors { get; set; }


        public RelayCommand<InspectorVM> SetInspectorCommand { get; set; }
        public RelayCommand<InspectorVM> RemoveInspectorCommand { get; set; }
        public RelayCommand<Window> SaveChangesCommand { get; set; }
        public RelayCommand<Window> CloseWindowCommand { get; set; }

        private InspectorVM _selectedAvailableInspector { get; set; }
        private InspectorVM _selectedChosenInspector { get; set; }
        private InspectionVM _selectedInspection { get; set; }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        public AvailableInspectorViewModel()
        {
            InitializeContext();

            ChosenInspectors = new ObservableCollection<InspectorVM>();

            SetInspectorCommand = new RelayCommand<InspectorVM>(AddInspector);
            RemoveInspectorCommand = new RelayCommand<InspectorVM>(RemoveInspector);
            SaveChangesCommand = new RelayCommand<Window>(SaveChanges);
            CloseWindowCommand = new RelayCommand<Window>(CloseWindow);
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


        internal void InitializeContext()
        {
            _context = new CustomFSContext();
        }

        public void SetContextInspectionId(CustomFSContext context, InspectionVM inspection)
        {
            _context = context;
            //if (inspectionId > 0)
            //{
                _selectedInspection = inspection;
                AvailableInspectors = _context.InspectorCrud.GetAllInspectorsFilteredByAvailability(
                    new List<DateTime>{
                    _selectedInspection.InspectionDate.StartDate,
                    _selectedInspection.InspectionDate.EndDate
                });
                ChosenInspectors = inspection.Inspectors;
                RemovedInspectors = new ObservableCollection<InspectorVM>();
                RaisePropertyChanged(nameof(AvailableInspectors));
                RaisePropertyChanged(nameof(ChosenInspectors));
            //}
            //else
            //{
            //    _selectedInspection = new InspectionVM(new Inspection());
            //    _selectedInspection.InspectionDate = inspectionDate;
            //    AvailableInspectors = _customFSContext.InspectorCrud.GetAllInspectorsFilteredByAvailability(
            //        new List<DateTime>{
            //        _selectedInspection.InspectionDate.StartDate,
            //        _selectedInspection.InspectionDate.EndDate
            //    });
            //    ChosenInspectors = new ObservableCollection<InspectorVM>();
            //    RemovedInspectors = new ObservableCollection<InspectorVM>();
            //}
            
        }

        private void AddInspector(InspectorVM inspectorAvailable)
        {
            if (inspectorAvailable != null)
            {
                ChosenInspectors.Add(inspectorAvailable);
                AvailableInspectors.Remove(inspectorAvailable);
            } else
            {
                MessageBox.Show("Geen inspecteur geselecteerd.");
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
                foreach (InspectorVM inspectorVM in _selectedInspection.Inspectors)
                {
                    if (inspectorVM.Id == inspectorChosen.Id)
                        RemovedInspectors.Add(inspectorChosen);
                }
                ChosenInspectors.Remove(inspectorChosen);
                AvailableInspectors.Add(inspectorChosen);
            } else
            {
                MessageBox.Show("Geen inspecteur geselecteerd.");
            }
        }

        private void SaveChanges(Window window)
        {
            // moet nog gefixt worden

            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Wilt u de veranderingen opslaan?", "Bevestigen", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    _selectedInspection.Inspectors = ChosenInspectors;
                    foreach (InspectorVM inspectorVM in ChosenInspectors)
                    {
                        for (var start = _selectedInspection.InspectionDate.StartDate; start <= _selectedInspection.InspectionDate.EndDate; start = start.AddDays(1))
                        {
                            ScheduleItemVM scheduleItemVM = new ScheduleItemVM(new ScheduleItem())
                            {
                                Inspector = inspectorVM.ToModel(),
                                Scheduled = true,
                                Date = (DateTime?)start,
                                ScheduleStartTime = _selectedInspection.InspectionDate.StartTime,
                                ScheduleEndTime = _selectedInspection.InspectionDate.EndTime
                            };
                            _context.ScheduleItems.Add(scheduleItemVM.ToModel());
                        }
                    }

                    
                    _context.ScheduleItemCrud.RemoveScheduleItemsByInspectorList(RemovedInspectors, _selectedInspection);
                    

                    window.Close();
                    Messenger.Default.Send(true, "UpdateAvailableList");
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        private void CloseWindow(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Sluiten zonder opslaan?", "Bevestiging annulering", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                window.Close();
            }
        }
    }
}
