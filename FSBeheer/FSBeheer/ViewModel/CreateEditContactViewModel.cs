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

        private CustomerVM Customer { get; set; }

        public RelayCommand<Window> SaveChangesContactCommand { get; set; }

        public RelayCommand<Window> DiscardContactCommand { get; set; }

        public RelayCommand<Window> DeleteContactCommand { get; set; }

        private ContactVM tempContact;

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        public CreateEditContactViewModel()
        {
            SaveChangesContactCommand = new RelayCommand<Window>(SaveChangesContact);
            DiscardContactCommand = new RelayCommand<Window>(DiscardContact);
            DeleteContactCommand = new RelayCommand<Window>(DeleteContact);
        }

        public void SetContact(CustomerVM customer)
        {
            tempContact = customer.Contact;
            Customer = customer;
            Contact = customer.Contact;
            RaisePropertyChanged(nameof(Contact));
        }

        private void SaveChangesContact(Window window)
        {
            if (!ContactIsValid()) return;

            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Wijzigingen opslaan?", "Bevestiging opslaan", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    try
                    {
                        window.Close();
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

            if (!Regex.Match(Contact.PhoneNumber, @"^(\+[0-9]{9})$").Success)
            {
                MessageBox.Show("De ingevoerde telefoonnummer is onjuist.");
                return false;
            }

            return true;
        }

        private void DiscardContact(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Sluiten zonder opslaan?", "Bevestig annulering", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                Contact = tempContact;
                window.Close();
            }
        }

        private void DeleteContact(Window window)
        {
            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Geselecteerde contactpersoon verwijderen?", "Bevestiging verwijdering", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    if (!string.IsNullOrEmpty(Customer.Contact.Name) || !string.IsNullOrEmpty(Customer.Contact.PhoneNumber) || !string.IsNullOrEmpty(Customer.Contact.Email))
                    {
                        Customer.Contact.IsDeleted = true;
                        Customer.Contact = new ContactVM();
                    }
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
