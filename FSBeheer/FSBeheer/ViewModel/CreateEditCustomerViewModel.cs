using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class CreateEditCustomerViewModel : ViewModelBase
    {
        public CustomerVM Customer { get; set; }

        public RelayCommand EditCommand { get; set; }

        public RelayCommand AddCommand { get; set; }

        public RelayCommand<Window> DiscardCommand { get; set; }

        // prop ContactVM

        private CustomFSContext _Context;

        public CreateEditCustomerViewModel(CustomerVM SelectedCustomer)
        {
            _Context = new CustomFSContext();
            EditCommand = new RelayCommand(ModifyCustomer);
            AddCommand = new RelayCommand(AddCustomer);
            DiscardCommand = new RelayCommand<Window>(Discard);

            // try catch
            if (SelectedCustomer != null)
            {
                Customer = SelectedCustomer;
                // contact van deze customer
            }
            else
            {
                Customer = new CustomerVM();
                // Contact aanmaken
            }
        }

        private void AddCustomer()
        {
            // not tested yet
            _Context.CustomerCrud.GetGetAllCustomerVMs().Add(Customer);
            _Context.CustomerCrud.Add(Customer);
        }

        // Not tested yet
        private void ModifyCustomer() => _Context.CustomerCrud.Modify(Customer);


        // Not tested yet
        private void Discard(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Close without saving?","Confirm discard", MessageBoxButton.OKCancel);
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
