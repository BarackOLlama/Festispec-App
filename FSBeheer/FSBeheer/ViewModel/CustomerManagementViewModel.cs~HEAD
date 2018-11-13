using FSBeheer.VM;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBeheer.View;

namespace FSBeheer.ViewModel
{
    public class CustomerManagementViewModel
    {
        private CustomFSContext CustomFSContext;
        public ObservableCollection<CustomerVM> Customers { get; set; }

        public RelayCommand CreateEditCustomerWindow { get; set; }

        public CustomerVM SelectedCustomer { get; set; }

        public CustomerManagementViewModel()
        {
            CustomFSContext = new CustomFSContext();
            Customers = CustomFSContext.GetCustomers();

            CreateEditCustomerWindow = new RelayCommand(OpenCreateEditCustomer);
        }

        private void OpenCreateEditCustomer()
        {
            new CreateEditCustomerView().Show();
        }
    }
}
