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
    public class CreateEditCustomerViewModel : ViewModelBase
    {
        public ContactVM Contact { get; set; }

        public CustomerVM Customer { get; set; }
        public RelayCommand EditContactWindowCommand { get; set; }

        public RelayCommand<Window> SaveChangesCommand { get; set; }
        public RelayCommand<Window> DiscardCommand { get; set; }
        public RelayCommand<Window> DeleteCustomerCommand { get; set; }

        private CustomFSContext _context;

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        public CreateEditCustomerViewModel()
        {
            Messenger.Default.Register<bool>(this, "UpdateContactList", cl => RaisePropertyChanged(nameof(Customer)));
            _context = new CustomFSContext();

            SaveChangesCommand = new RelayCommand<Window>(SaveChanges);
            DiscardCommand = new RelayCommand<Window>(Discard);
            DeleteCustomerCommand = new RelayCommand<Window>(DeleteCustomer);
            EditContactWindowCommand = new RelayCommand(OpenCreateEditContact);
        }

        private bool CustomerIsValid()
        {
            if (Customer.Address == null)
            {
                MessageBox.Show("Een klant moet een adres hebben.");
                return false;
            }

            if (Customer.Address.Trim() == string.Empty)
            {
                MessageBox.Show("Een klant moet een adres hebben.");
                return false;
            }

            if (Customer.ChamberOfCommerceNumber == null)
            {
                MessageBox.Show("Een klant moet een KVK nummer hebben.");
                return false;
            }

            if (!Regex.IsMatch(Customer.ChamberOfCommerceNumber+"", @"^\d{8,}$"))
            {
                MessageBox.Show("De ingevoerde KVK is niet valide.\nEen KVK nummer moet minstens uit acht cijfers bestaan.");
                return false;
            }

            if (Customer.City == null)
            {
                MessageBox.Show("Een klant moet een stad hebben.");
                return false;
            }

            if (Customer.City.Trim() == string.Empty)
            {
                MessageBox.Show("Een klant moet een stad hebben.");
                return false;
            }

            if (Customer.Name == null)
            {
                MessageBox.Show("Een klant moet een naam hebben.");
                return false;
            }

            if (Customer.Name.Trim() == string.Empty)
            {
                MessageBox.Show("Een klant moet een naam hebben.");
                return false;
            }

            if (Customer.ZipCode == null)
            {
                MessageBox.Show("Een klant moet een postcode hebben.");
                return false;
            }

            if (Customer.ZipCode.Trim() == string.Empty)
            {
                MessageBox.Show("Een klant moet een postcode hebben.");
                return false;
            }

            if (!Regex.Match(Customer.ZipCode, "^[0-9]{4}[A-Z]{2}$").Success)
            {
                MessageBox.Show("De ingevoerde postcode is incorrect.");
                return false;
            }

            return true;
        }

        private void OpenCreateEditContact()
        {
            if(IsInternetConnected())
                new CreateEditContactView(Customer).Show();
            else
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
        }


        public void SetCustomer(CustomerVM customer)
        {
            if (customer == null)
            {
                Customer = new CustomerVM
                {
                    Contact = new ContactVM()
                };
                _context.Customers.Add(Customer.ToModel());
                RaisePropertyChanged(nameof(Customer));
            }
            else
            {
                Customer = new CustomerVM(_context.Customers.FirstOrDefault(c => c.Id == customer.Id));
                RaisePropertyChanged(nameof(Customer));
            }
            RaisePropertyChanged(nameof(Customer));
        }

        private void SaveChanges(Window window)
        {
            if (!CustomerIsValid()) return;

            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Wijzigingen opslaan?", "Bevestig opslaan", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    try
                    {
                        Customer.StartingDate = DateTime.Now.Date;
                        _context.SaveChanges();
                        window.Close();

                        Messenger.Default.Send(true, "UpdateCustomerList");
                    }
                    catch (Exception error)
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

        private void Discard(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Sluiten zonder op te slaan?", "Bevestig annuleren", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _context.Dispose();
                Customer = null;
                window.Close();
            }
        }

        private void DeleteCustomer(Window window)
        {
            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Verwijder de geselecteerde klant?", "Bevestig verwijdering", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    Customer.IsDeleted = true;
                    Customer.Contact.IsDeleted = true;

                    _context.SaveChanges();
                    window.Close();

                    Messenger.Default.Send(true, "UpdateCustomerList");
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }
    }
}
