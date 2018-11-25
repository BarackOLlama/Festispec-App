using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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

        private string _FilterText;
        public string FilterText { get { return _FilterText; }
            set { _FilterText = value; } }

        public RelayCommand<Window> BackHomeCommand { get; set; }
        public RelayCommand EditEventCommand { get; set; }
        public RelayCommand NewEventCommand { get; set; }

        public EventManagementViewModel()
        {
            _Context = new CustomFSContext();

            BackHomeCommand = new RelayCommand<Window>(CloseAction);

            Events = _Context.EventCrud.GetAllEventVMs();
        }

        private void CloseAction(Window window)
        {
            window.Close();
        }

        public void FilterList(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                Events = _Context.EventCrud.GetAllEventVMs();
            }
            else
            {
                Events = _Context.EventCrud.GetFilteredEventsByString(filter);
            }
            RaisePropertyChanged(nameof(Events));
        }
    }
}
