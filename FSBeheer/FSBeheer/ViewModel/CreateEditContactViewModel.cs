using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class CreateEditContactViewModel : ViewModelBase
    {
        public ContactVM Contact { get; set; }

        public CustomerVM _linkCustomer { get; set; }

        public RelayCommand SaveChangesContactCommand { get; set; }

        public RelayCommand<Window> DiscardContactCommand { get; set; }

        public RelayCommand<Window> DeleteContactCommand { get; set; }

        private CustomFSContext _context;

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        public CreateEditContactViewModel()
        {
            _context = new CustomFSContext();
            SaveChangesContactCommand = new RelayCommand(SaveChangesContact);
            DiscardContactCommand = new RelayCommand<Window>(DiscardContact);
            DeleteContactCommand = new RelayCommand<Window>(DeleteContact);
        }

        public void SetContact(ContactVM contact, CustomerVM customer)
        {
            _linkCustomer = customer;
            if (contact == null)
            {
                Contact = new ContactVM()
                {
                    CustomerId = _linkCustomer.Id
                };
                _context.Contacts.Add(Contact.ToModel());
                RaisePropertyChanged(nameof(Contact));
            }
            else
            {
                Contact = new ContactVM(_context.Contacts.FirstOrDefault(c => c.Id == contact.Id));
                RaisePropertyChanged(nameof(Contact));
            }
        }

        private void SaveChangesContact()
        {
            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Wijzigingen opslaan?", "Bevestiging opslaan", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    // TODO: Als je nieuw klant aanmaakt en niet saved en dan contact aanmaakt zit hij nog niet in de database en krijg je een error
                    try
                    {
                        _context.ContactCrud.GetAllContactVMs().Add(Contact);
                        _context.SaveChanges();
                    }
                    catch
                    {
                        return;
                    }

                    Messenger.Default.Send(true, "UpdateContactList");
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        private void DiscardContact(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Sluiten zonder opslaan?", "Bevestig annulering", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _context.Dispose();
                Contact = null;
                window?.Close();
            }
        }

        private void DeleteContact(Window window)
        {
            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Geselecteerde contactpersoon verwijderen?", "Bevestiging verwijdering", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    Contact.IsDeleted = true;
                    _context.SaveChanges();
                    window.Close();

                    Messenger.Default.Send(true, "UpdateContactList");
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }
    }
}
