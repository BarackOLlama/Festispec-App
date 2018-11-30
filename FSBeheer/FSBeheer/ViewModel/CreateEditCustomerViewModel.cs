using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class CreateEditCustomerViewModel : ViewModelBase
    {
        public CustomerVM Customer { get; set; }

        public RelayCommand SaveChangesCommand { get; set; }

        public RelayCommand<Window> DiscardCommand { get; set; }

        public RelayCommand<Window> DeleteCustomerCommand { get; set; }

        public RelayCommand CreateContactWindowCommand { get; set; }

        public RelayCommand EditContactWindowCommand { get; set; }

        public ObservableCollection<ContactVM> Contacts { get; set; }

        private ContactVM _selectedContact { get; set; }

        private CustomFSContext _Context;

        public CreateEditCustomerViewModel()
        {
            Messenger.Default.Register<bool>(this, "UpdateContactList", cl => Init()); // registratie, ontvangt (recipient is dit zelf) Observable Collection van CustomerVM en token is CustomerList, en voeren uiteindelijk init() uit, stap I

            Init();
            SaveChangesCommand = new RelayCommand(SaveChanges);
            DiscardCommand = new RelayCommand<Window>(Discard);
            DeleteCustomerCommand = new RelayCommand<Window>(DeleteCustomer);
            CreateContactWindowCommand = new RelayCommand(OpenCreateContact);
            EditContactWindowCommand = new RelayCommand(OpenEditContact);
        }

        internal void Init()
        {
            _Context = new CustomFSContext();
            if (Customer != null)
            {
                Contacts = _Context.ContactCrud.GetContactByCustomer(Customer);
                RaisePropertyChanged(nameof(Contacts));
            }
        }

        private void OpenCreateContact()
        {
            new CreateEditContactView(null, Customer).Show();
        }

        private void OpenEditContact()
        {
            if (_selectedContact == null)
            {
                MessageBox.Show("No contact selected");
            }
            else
            {
                new CreateEditContactView(_selectedContact, Customer).Show();
            }
        }

        public ContactVM SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                _selectedContact = value;
                base.RaisePropertyChanged(nameof(SelectedContact));
            }
        }


        public void SetCustomer(CustomerVM customer)
        {
            if (customer == null)
            {
                Customer = new CustomerVM();
                _Context.Customers.Add(Customer.ToModel());
                RaisePropertyChanged(nameof(Customer)); // a sign that a property has changed for viewing
            }
            else
            {
                Customer = new CustomerVM(_Context.Customers.FirstOrDefault(c => c.Id == customer.Id));
                RaisePropertyChanged(nameof(Customer));
            }
            Contacts = _Context.ContactCrud.GetContactByCustomer(Customer); // TODO kan beter
            RaisePropertyChanged(nameof(Contacts));
        }

        private void SaveChanges()
        {
            MessageBoxResult result = MessageBox.Show("Save changes?", "Confirm action", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _Context.CustomerCrud.GetAllCustomers().Add(Customer);
                _Context.SaveChanges();

                Messenger.Default.Send(true, "UpdateCustomerList"); // Stuurt object true naar ontvanger, die dan zijn methode init() uitvoert, stap II
            }
        }

        private void Discard(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Close without saving?", "Confirm discard", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _Context.Dispose();
                Customer = null;
                window.Close();
            }
        }

        /// <summary>
        /// Set Customer on Deleted, and also all contacts connected to it (fields)
        /// </summary>
        /// <param name="window"></param>
        private void DeleteCustomer(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Delete the selected customer?", "Confirm Delete", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                Customer.IsDeleted = true;
                foreach (var e in Contacts)
                {
                    e.IsDeleted = true;
                }
                _Context.SaveChanges(); // TODO: Changes of last changes to customer stays, do we want that?
                window.Close();

                Messenger.Default.Send(true, "UpdateCustomerList");
            }
        }
    }
}
