using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.InteropServices;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class ScheduleManagementViewModel : ViewModelBase
    {
        private CustomFSContext _Context;

        public ObservableCollection<InspectorVM> Inspectors { get; set; }
        public InspectorVM SelectedInspector { get; set; }

        public ObservableCollection<AvailabilityVM> Availabilities { get; set; }
        public ObservableCollection<AvailabilityVM> SelectedAvailabilities { get; set; }

        public RelayCommand<Window> BackHomeCommand { get; set; }
        public RelayCommand GetAvailabilityCommand { get; set; }
        public RelayCommand NewAvailabilityCommand { get; set; }
        public RelayCommand EditAvailabilityCommand { get; set; }
        public RelayCommand DeleteAvailabilityCommand { get; set; }
        public RelayCommand CanExecuteChangedCommand { get; set; }
        public RelayCommand<object> DatagridSelectionChangedCommand { get; set; }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        public ScheduleManagementViewModel()
        {
            Init();
            Messenger.Default.Register<bool>(this, "UpdateSchedule", e => Init());

            BackHomeCommand = new RelayCommand<Window>(CloseAction);
            GetAvailabilityCommand = new RelayCommand(GetAvailabilities);
            DeleteAvailabilityCommand = new RelayCommand(DeleteAvailability, AvailabilitySelected);
            NewAvailabilityCommand = new RelayCommand(NewAvailability, InspectorSelected);
            CanExecuteChangedCommand = new RelayCommand(CanExecuteChanged);
            DatagridSelectionChangedCommand = new RelayCommand<object>(DataGridSelectionChanged);
        }

        public void Init()
        {
            _Context = new CustomFSContext();
            GetData();
        }

        private void GetData()
        {
            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddDays(1)
            };
            if (IsInternetConnected())
            {
                Inspectors = _Context.InspectorCrud.GetAllInspectors();
                cache.Set("inspectors", Inspectors, policy);
            }
            else
            {
                Inspectors = cache["inspectors"] as ObservableCollection<InspectorVM>;
                if (Inspectors == null)
                {
                    Inspectors = new ObservableCollection<InspectorVM>();
                }
            }
            RaisePropertyChanged(nameof(Inspectors));
        }

        public void GetAvailabilities()
        {
            if (IsInternetConnected())
            {
                if (SelectedInspector != null)
                {
                    Availabilities = _Context.AvailabilityCrud.GetAllAvailabilitiesByInspector(SelectedInspector.Id);
                }
                RaisePropertyChanged(nameof(Availabilities));
            }
            else
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
        }

        private void DataGridSelectionChanged(object dataset)
        {
            if(dataset != null)
                SelectedAvailabilities = new ObservableCollection<AvailabilityVM>((dataset as IEnumerable).Cast<AvailabilityVM>());
        }

        private void CanExecuteChanged()
        {
            DeleteAvailabilityCommand.RaiseCanExecuteChanged();
            NewAvailabilityCommand.RaiseCanExecuteChanged();

            RaisePropertyChanged(nameof(SelectedInspector));
            RaisePropertyChanged(nameof(SelectedAvailabilities));
        }

        private bool InspectorSelected()
        {
            return SelectedInspector != null;
        }

        private bool AvailabilitySelected()
        {
            return SelectedAvailabilities != null && SelectedAvailabilities.Count != 0;
        }

        private void CloseAction(Window window)
        {
            window.Close();
        }

        private void DeleteAvailability()
        {
            if (IsInternetConnected())
            {
                var result = MessageBox.Show("Weet u zeker dat u deze roosteritems wilt verwijderen?\nLet op: Alleen roosteritems van het type Verlof kunnen hier verwijderd worden.", "Bevestigen", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    foreach(var item in SelectedAvailabilities)
                    {

                    }
                }
            }
            else
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
        }

        private void NewAvailability()
        {
            if (IsInternetConnected())
            {
                new CreateEditScheduleView().ShowDialog();
            }
            else
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
        }
    }
}
