using FSBeheer.Model;
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
    public class CreateEditEventViewModel : ViewModelBase
    {
        private CustomFSContext _Context { get; set; }
        public string Title { get; set; }
        public EventVM Event { get; set; }
        public ObservableCollection<CustomerVM> Customers { get; set; }
        public CustomerVM SelectedCustomer { get; set; }
        public string WarningText { get; set; }

        public RelayCommand<Window> SaveChangesCommand { get; set; }
        public RelayCommand<Window> DiscardChangesCommand { get; set; }
        public RelayCommand<Window> DeleteEventCommand { get; set; }
        public RelayCommand CanExecuteChangedCommand { get; set; }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        public CreateEditEventViewModel()
        {
            _Context = new CustomFSContext();
            Customers = _Context.CustomerCrud.GetAllCustomers();

            SaveChangesCommand = new RelayCommand<Window>(SaveChanges, SaveAllowed);
            DiscardChangesCommand = new RelayCommand<Window>(DiscardChanges);
            DeleteEventCommand = new RelayCommand<Window>(DeleteEvent);
            CanExecuteChangedCommand = new RelayCommand(CanExecuteChanged);
        }

        public void SetEvent(int eventId)
        {
            if(eventId == -1)
            {
                Event = new EventVM(new Event());
                _Context.Events.Add(Event.ToModel());
                SelectedCustomer = null;
                Title = "Evenement aanmaken";
            }
            else
            {
                Event = _Context.EventCrud.GetEventById(eventId);
                SelectedCustomer = _Context.CustomerCrud.GetCustomerById(Event.Customer.Id);
                Title = "Evenement wijzigen";
            }
            RaisePropertyChanged(nameof(Event));
            RaisePropertyChanged(nameof(SelectedCustomer));
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
            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Weet u zeker dat u dit evenement op wilt slaan?", "Bevestigen", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    _Context.SaveChanges();
                    Messenger.Default.Send(true, "UpdateEventList");
                    CloseAction(window);
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
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
            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Weet u zeker dat u dit evenement wilt verwijderen?", "Bevestigen", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    _Context.EventCrud.Delete(Event);
                    Messenger.Default.Send(true, "UpdateEventList");
                    CloseAction(window);
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }
    }
}
