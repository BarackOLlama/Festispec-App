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

        public RelayCommand CreateEditCustomerWindowCommand { get; set; }
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

            CreateEditCustomerWindowCommand = new RelayCommand(OpenCreateEditCustomer);
            DeleteCommand = new RelayCommand(DeleteCustomer);
        }

        private void OpenCreateEditCustomer()
        {
            //this was set the wrong window. CreateEditCustomer still needs to be made. Window that was being opened was CreateEditInspection
            //new CreateEditCustomerView().Show();
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
