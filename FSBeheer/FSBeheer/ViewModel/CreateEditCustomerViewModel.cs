using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class CreateEditCustomerViewModel : ViewModelBase
    {
        public CustomerVM Customer { get; set; }

        public RelayCommand SaveChangesCommand { get; set; }

        public RelayCommand<Window> DiscardCommand { get; set; }

        // prop ContactVM

        private CustomFSContext _Context;

        public CreateEditCustomerViewModel()
        {
            _Context = new CustomFSContext();
            SaveChangesCommand = new RelayCommand(SaveChanges);
            DiscardCommand = new RelayCommand<Window>(Discard);
        }


        public void SetCustomer(CustomerVM customer)
        {
            if (customer == null)
            {
                Customer = new CustomerVM();
                _Context.Customers.Add(Customer.ToModel());
                RaisePropertyChanged(nameof(Customer));
            }
            else
            {
                Customer = new CustomerVM(_Context.Customers.FirstOrDefault(c => c.Id == customer.Id));
                RaisePropertyChanged(nameof(Customer));
            }
        }

        private void SaveChanges()
        {
            _Context.CustomerCrud.GetAllCustomerVMs().Add(Customer);
            _Context.SaveChanges();

            Messenger.Default.Send(true, "UpdateCustomerList"); // Stuurt object true naar ontvanger, die dan zijn methode init() uitvoert, stap II
        }

        private void Discard(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Close without saving?", "Confirm discard", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                _Context.Dispose();
                Customer = null;
                window?.Close();
            }
        }

        // TODO: Connect to a new contact person when adding a customer 
    }
}
