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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class CreateEditEventViewModel : ViewModelBase
    {
        private CustomFSContext _context { get; set; }
        public string Title { get; set; }
        public EventVM Event { get; set; }
        public ObservableCollection<CustomerVM> Customers { get; set; }
        public int SelectedIndex { get; set; }
        public string WarningText { get; set; }
        public bool CurrentlyEditingEvent { get; set; }

        public RelayCommand<Window> SaveChangesCommand { get; set; }
        public RelayCommand<Window> CloseWindowCommand { get; set; }
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
            _context = new CustomFSContext();
            Customers = _context.CustomerCrud.GetAllCustomers();

            SaveChangesCommand = new RelayCommand<Window>(SaveChanges);
            CloseWindowCommand = new RelayCommand<Window>(CloseWindow);
            DeleteEventCommand = new RelayCommand<Window>(DeleteEvent);
            CanExecuteChangedCommand = new RelayCommand(CanExecuteChanged);
        }

        public void SetEvent(int eventId = -1)
        {
            if (eventId == -1)
            {
                Event = new EventVM();
                _context.Events.Add(Event.ToModel());
                Title = "Evenement aanmaken";
                CurrentlyEditingEvent = false;
                RaisePropertyChanged(nameof(CurrentlyEditingEvent));
            }
            else
            {
                Event = _context.EventCrud.GetEventById(eventId);
                Title = "Evenement wijzigen";
                CurrentlyEditingEvent = true;
                RaisePropertyChanged(nameof(CurrentlyEditingEvent));
            }
            if (Event.Customer.ToModel() != null)
                SelectedIndex = GetIndex(Event.Customer, Customers);
            else
                SelectedIndex = -1;
            RaisePropertyChanged(nameof(SelectedIndex));
            RaisePropertyChanged(nameof(Event));
            RaisePropertyChanged(nameof(Title));
        }

        private int GetIndex(CustomerVM customer, ObservableCollection<CustomerVM> customerList)
        {
            for (int i = 0; i < customerList.Count; i++)
                if (customerList[i].Id == customer.Id)
                    return i;
            return -1;
        }

        private void CloseWindow(Window window)
        {
            var result = MessageBox.Show("Scherm sluiten?", "Scherm sluiten", MessageBoxButton.OKCancel);
            if(result == MessageBoxResult.OK)
            {
                window.Close();
            }
        }

        private void CanExecuteChanged()
        {
            SaveChangesCommand.RaiseCanExecuteChanged();
        }

        private bool SaveAllowed()
        {
            if (Event != null)
            {
                if (string.IsNullOrEmpty(Event.Name))
                {
                    MessageBox.Show("Het veld Naam mag niet leeg zijn");
                    return false;
                }
                else if (string.IsNullOrEmpty(Event.Address))
                {
                    MessageBox.Show("Het veld Adres mag niet leeg zijn");
                    return false;
                }
                else if (string.IsNullOrEmpty(Event.City))
                {
                    MessageBox.Show("Het veld Plaats mag niet leeg zijn");
                    return false;
                }
                else if (string.IsNullOrEmpty(Event.Zipcode))
                {
                    MessageBox.Show("Het veld Postcode mag niet leeg zijn");
                    return false;
                } else if (!Regex.IsMatch(Event.Zipcode, @"^[0-9]{4} ?[A-Z]{2}$"))
                {
                    MessageBox.Show("De postcode is niet valide.\nEen valide postcode is bijvoorbeeld: 1245AB.");
                    return false;
                } else if (Event.Customer == null)
                {
                    MessageBox.Show("Een evenement moet een klant hebben.");
                    return false;
                } else if (Event.EventDate.StartDate == null)
                {
                    MessageBox.Show("Een evenement moet een startdatum hebben.");
                    return false;
                }else if (Event.EventDate.EndDate == null)
                {
                    MessageBox.Show("Een evenement moet een einddatum hebben.");
                    return false;
                }else if (Event.EventDate.EndDate < Event.EventDate.StartDate)
                {
                    MessageBox.Show("De einddatum van een evenement mag niet voor de startdatum zijn.");
                    return false;
                }else if (SelectedIndex == -1)
                {
                    MessageBox.Show("Een evenement moet een klant hebben.");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                MessageBox.Show("De onderliggende event is NULL. Rapporteer deze bug.");
                return false;
            }
        }

        private void SaveChanges(Window window)
        {
            if (!SaveAllowed()) return;

            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Weet u zeker dat u dit evenement op wilt slaan?", "Bevestigen", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    try
                    {
                        _context.SaveChanges();
                        _context.Events.RemoveRange(_context.Events.Where(e => e.Name == null));
                        _context.Customers.RemoveRange(_context.Customers.Where(c => c.Name == null));
                        _context.SaveChanges();
                        Messenger.Default.Send(true, "UpdateEventList");
                        window.Close();
                    }
                    catch (System.Exception error)
                    {
                        if (error is System.Data.Entity.Infrastructure.DbUpdateException)
                        {
                            MessageBox.Show("U probeerd een duplicaat toe te voegen in het systeem");
                        }

                    }

                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        private void DeleteEvent(Window window)
        {
            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Weet u zeker dat u dit evenement wilt verwijderen?", "Verwijder event", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    _context.EventCrud.Delete(Event);
                    Messenger.Default.Send(true, "UpdateEventList");
                    window.Close();
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }
    }
}
