using FSBeheer.Model;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class CreateEditEventViewModel : ViewModelBase
    {
        private CustomFSContext _Context { get; set; }
        public string Title { get; set; }
        public EventVM Event { get; set; }
        public ObservableCollection<CustomerVM> Customers { get; set; }
        public string WarningText { get; set; }

        public RelayCommand<Window> SaveChangesCommand { get; set; }
        public RelayCommand<Window> DiscardChangesCommand { get; set; }
        public RelayCommand<Window> DeleteEventCommand { get; set; }
        public RelayCommand CanExecuteChangedCommand { get; set; }

        public CreateEditEventViewModel()
        {
            _Context = new CustomFSContext();

            SaveChangesCommand = new RelayCommand<Window>(SaveChanges, SaveAllowed);
            DiscardChangesCommand = new RelayCommand<Window>(DiscardChanges);
            DeleteEventCommand = new RelayCommand<Window>(DeleteEvent);
            CanExecuteChangedCommand = new RelayCommand(CanExecuteChanged);
        }

        public void SetEvent(int eventId)
        {
            if (eventId == -1)
            {
                Event = new EventVM(new Event());
                _Context.Events.Add(Event.ToModel());
                Title = "Evenement aanmaken";
            }
            else
            {
                Event = _Context.EventCrud.GetEventById(eventId);
                Title = "Evenement wijzigen";
            }
            RaisePropertyChanged(nameof(Event));
            RaisePropertyChanged(nameof(Title));
        }

        private void CloseAction(Window window)
        {
            _Context.Dispose();
            window.Close();
        }

        private void CanExecuteChanged()
        {
            SaveChangesCommand.RaiseCanExecuteChanged();
        }

        private bool SaveAllowed(object args)
        {
            if (string.IsNullOrEmpty(Event.Name))
            {
                WarningText = "Het veld Naam mag niet leeg zijn";
                RaisePropertyChanged(nameof(WarningText));
                return false;
            }
            else if (string.IsNullOrEmpty(Event.Address))
            {
                WarningText = "Het veld Adres mag niet leeg zijn";
                RaisePropertyChanged(nameof(WarningText));
                return false;
            }
            else if (string.IsNullOrEmpty(Event.City))
            {
                WarningText = "Het veld Plaats mag niet leeg zijn";
                RaisePropertyChanged(nameof(WarningText));
                return false;
            }
            else if (string.IsNullOrEmpty(Event.Zipcode))
            {
                WarningText = "Het veld Postcode mag niet leeg zijn";
                RaisePropertyChanged(nameof(WarningText));
                return false;
            }
            else if (Event.Customer == null)
            {
                WarningText = "U moet een klant kiezen";
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

        private void SaveChanges(Window window)
        {
            MessageBoxResult result = MessageBox.Show(" Weet u zeker dat u dit evenement op wilt slaan?", "Bevestigen", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _Context.SaveChanges();
                Messenger.Default.Send(true, "UpdateEventList");
                CloseAction(window);
            }
        }

        private void DiscardChanges(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Weet u zeker dat u wilt afsluiten zonder op te slaan?", "Bevestigen", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                CloseAction(window);
            }
        }

        private void DeleteEvent(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Weet u zeker dat u dit evenement wilt verwijderen?", "Bevestigen", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _Context.EventCrud.Delete(Event);
                Messenger.Default.Send(true, "UpdateEventList");
                CloseAction(window);
            }
        }
    }
}
