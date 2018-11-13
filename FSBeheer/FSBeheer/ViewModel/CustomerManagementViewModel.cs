using FSBeheer.VM;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBeheer.View;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class CustomerManagementViewModel
    {
        private CustomFSContext CustomFSContext;
        public ObservableCollection<CustomerVM> Customers { get; set; }

        public RelayCommand CreateEditCustomerWindowCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }

        public CustomerVM SelectedCustomer { get; set; } // doorgeven aan edit


        public CustomerManagementViewModel()
        {
            CustomFSContext = new CustomFSContext();
            Customers = CustomFSContext.GetCustomers();

            CreateEditCustomerWindowCommand = new RelayCommand(OpenCreateEditCustomer);
            DeleteCommand = new RelayCommand(DeleteCustomer);
        }

        private void OpenCreateEditCustomer()
        {
            new CreateEditCustomerView().Show();
        }

        private void DeleteCustomer()
        {
            if (SelectedCustomer != null)
            {
                CustomFSContext.CustomerCrud.GetCustomerVMs.Remove(SelectedCustomer);
                CustomFSContext.CustomerCrud.Delete(SelectedCustomer);
            }
        }
    }
}
