using FSBeheer.Model;
using FSBeheer.VM;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class CreateEditCustomerViewModel
    {
        public CustomerVM Customer { get; set; }

        private RelayCommand EditCommand { get; set; }

        private RelayCommand AddCommand { get; set; }

        private RelayCommand<Window> DiscardCommand { get; set; }

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
            _Context.CustomerCrud.GetCustomerVMs.Add(Customer);
            _Context.CustomerCrud.Add(Customer);
        }

        private void ModifyCustomer()
        {
            _Context.CustomerCrud.Modify(Customer);
        }


        // Test
        private void Discard(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to close without saving?","Confirm discard", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                _Context.Dispose();
                Customer = null; // Info reset
                window.Close();
            }
        }

        // TODO: Connect to a new contact person when adding a customer 
    }
}
