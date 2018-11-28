using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private CustomFSContext _Context;

        public CreateEditContactViewModel()
        {
            _Context = new CustomFSContext();
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
                _Context.Contacts.Add(Contact.ToModel());
                RaisePropertyChanged(nameof(Contact));
            }
            else
            {
                Contact = new ContactVM(_Context.Contacts.FirstOrDefault(c => c.Id == contact.Id));
                RaisePropertyChanged(nameof(Contact));
            }
        }

        private void SaveChangesContact()
        {
            MessageBoxResult result = MessageBox.Show("Save changes?", "Confirm action", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _Context.ContactCrud.GetAllContactVMs().Add(Contact);
                _Context.SaveChanges();

                Messenger.Default.Send(true, "UpdateContactList");
            }
        }

        private void DiscardContact(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Close without saving?", "Confirm discard", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _Context.Dispose();
                Contact = null;
                window?.Close();
            }
        }

        private void DeleteContact(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Delete the selected contactperson?", "Confirm Delete", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                Contact.IsDeleted = true;
                _Context.SaveChanges(); 
                window.Close();

                Messenger.Default.Send(true, "UpdateContactList");
            }
        }
    }
}
