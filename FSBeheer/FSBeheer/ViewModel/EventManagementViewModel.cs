using FSBeheer.View;
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
using System.Runtime.Caching;
using System.Runtime;
using System.Runtime.InteropServices;

namespace FSBeheer.ViewModel
{
    public class EventManagementViewModel : ViewModelBase
    {
        private CustomFSContext _Context;

        public ObservableCollection<EventVM> Events { get; set; }
        public EventVM SelectedEvent { get; set; }

        public RelayCommand<Window> BackHomeCommand { get; set; }
        public RelayCommand CreateEventCommand { get; set; }
        public RelayCommand EditEventCommand { get; set; }
        public RelayCommand CanExecuteChangedCommand { get; set; }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        public EventManagementViewModel()
        {
            Init();

            Messenger.Default.Register<bool>(this, "UpdateEventList", e => Init());

            BackHomeCommand = new RelayCommand<Window>(CloseAction);
            CreateEventCommand = new RelayCommand(OpenCreateEvent);
            EditEventCommand = new RelayCommand(OpenEditEvent, EventSelected);
            CanExecuteChangedCommand = new RelayCommand(CanExecuteChanged);
        }

        private void Init()
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
            if(IsInternetConnected())
            {
                Events = _Context.EventCrud.GetAllEvents();
                cache.Set("events", Events, policy);
            }
            else
            {
                Events = cache["events"] as ObservableCollection<EventVM>;
                if(Events == null)
                {
                    Events = new ObservableCollection<EventVM>();
                }
            }
            RaisePropertyChanged(nameof(Events));
        }

        private void CanExecuteChanged()
        {
            EditEventCommand.RaiseCanExecuteChanged();
        }

        private bool EventSelected()
        {
            return SelectedEvent != null;
        }

        private void CloseAction(Window window)
        {
            window.Close();
        }

        public void FilterList(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                Events = _Context.EventCrud.GetAllEvents();
            }
            else
            {
                Events = _Context.EventCrud.GetAllEventsFiltered(filter);
            }
            RaisePropertyChanged(nameof(Events));
        }

        private void OpenCreateEvent()
        {
            if (IsInternetConnected())
                new CreateEditEventView().Show();
            else
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
        }

        private void OpenEditEvent()
        {
            if (IsInternetConnected())
            {
                if (SelectedEvent != null)
                    new CreateEditEventView(SelectedEvent.Id).Show();
            }
            else
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
        }
    }
}
