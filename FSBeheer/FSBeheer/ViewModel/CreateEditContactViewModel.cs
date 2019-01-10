using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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
                Contact = new ContactVM(_context.Contacts.ToList().FirstOrDefault(c => c.Id == contact.Id));
                RaisePropertyChanged(nameof(Contact));
            }
        }

        private void SaveChangesContact()
        {

            if (!ContactIsValid()) return;

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
                        MessageBox.Show("Opslaan niet gelukt!");
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

        public bool ContactIsValid()
        {
            //determines whether the contact is valid and may be saved to the database

            if (Contact.Email == null)
            {
                MessageBox.Show("Een contactpersoon moet een e-mail adres hebben");
                return false;
            }

            if (Contact.Email.Trim() == string.Empty)
            {
                MessageBox.Show("Een contactpersoon moet een e-mail adres hebben");
                return false;
            }

            if (!new EmailAddressAttribute().IsValid(Contact.Email))
            {
                MessageBox.Show("Het ingevoerde e-mail adres is onjuist.");
                return false;
            }

            if (Contact.Name == null)
            {
                MessageBox.Show("Een contactpersoon moet een naam hebben.");
                return false;
            }

            if (Contact.Name.Trim() == string.Empty)
            {
                MessageBox.Show("Een contactpersoon moet een naam hebben.");
                return false;
            }

            if (Contact.PhoneNumber == null)
            {
                MessageBox.Show("Een contactpersoon moet een telefoonnummer hebben.");
                return false;
            }

            if (!Regex.Match(Contact.PhoneNumber.Trim(), @"^((((\+|(00))(31))6?)|(06))?[0-9]{8}$").Success)
            {
                MessageBox.Show("Het ingevoerde telefoonnummer is incorrect.");
                return false;
            }

            return true;
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
