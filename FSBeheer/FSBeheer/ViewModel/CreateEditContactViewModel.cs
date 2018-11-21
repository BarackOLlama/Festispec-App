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

        public RelayCommand SaveChangesCommand { get; set; }

        public RelayCommand<Window> DiscardCommand { get; set; }

        private CustomFSContext _Context;

        public CreateEditContactViewModel()
        {
            _Context = new CustomFSContext();
            SaveChangesCommand = new RelayCommand(SaveChanges);
            DiscardCommand = new RelayCommand<Window>(Discard);
        }

        public void SetContact(ContactVM contact)
        {
            if (contact == null)
            {
                Contact = new ContactVM();
                _Context.Contacts.Add(Contact.ToModel());
                RaisePropertyChanged(nameof(Contact));
            }
            else
            {
                Contact = new ContactVM(_Context.Contacts.FirstOrDefault(c => c.Id == contact.Id));
                RaisePropertyChanged(nameof(Contact));
            }
        }

        private void SaveChanges()
        {
            // _Context.ContactCrud.GetAllContactVMs().Add(Contact);
            _Context.SaveChanges();

            // Messenger.Default.Send(true, "UpdateContactList"); // Stuurt object true naar ontvanger, die dan zijn methode init() uitvoert, stap II
        }

        private void Discard(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Close without saving?", "Confirm discard", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                _Context.Dispose();
                Contact = null;
                window?.Close();
            }
        }
    }
}
