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
            Events = _Context.EventCrud.GetAllEvents();
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
            new CreateEditEventView().Show();
        }

        private void OpenEditEvent()
        {
            if(SelectedEvent != null)
            {
                new CreateEditEventView(SelectedEvent.Id).Show();
            }
        }
    }
}
