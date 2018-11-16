using FSBeheer.VM;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using FSBeheer.View;
using GalaSoft.MvvmLight;

namespace FSBeheer.ViewModel
{
    public class CustomerManagementViewModel : ViewModelBase
    {
        private CustomFSContext CustomFSContext;
        public ObservableCollection<CustomerVM> Customers { get; set; }

        public RelayCommand EditCustomerWindowCommand { get; set; }

        public RelayCommand CreateCustomerWindowCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }

        private CustomerVM _selectedCustomer { get; set; } 

        public CustomerVM SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                base.RaisePropertyChanged(nameof(SelectedCustomer));
            }
        }


        public CustomerManagementViewModel()
        {
            CustomFSContext = new CustomFSContext();
            Customers = CustomFSContext.CustomerCrud.GetGetAllCustomerVMs();

            CreateCustomerWindowCommand = new RelayCommand(OpenCreateCustomer);
            EditCustomerWindowCommand = new RelayCommand(OpenEditCustomer);
            DeleteCommand = new RelayCommand(DeleteCustomer);
        }

        // Standard way of doing this
        private void OpenCreateCustomer()
        {
            new CreateEditCustomerViewModel();
            new CreateEditCustomerView().Show();
        }
        private void OpenEditCustomer()
        {
            new CreateEditCustomerViewModel(_selectedCustomer);
            new CreateEditCustomerView().Show();
        }

        private void DeleteCustomer()
        {
            if (SelectedCustomer != null)
            {
                CustomFSContext.CustomerCrud.Delete(SelectedCustomer);
                Customers.Remove(SelectedCustomer);
            }
        }
    }
}
