﻿using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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

        ///// <summary>
        ///// Constructor when an existing customer is selected
        ///// </summary>
        ///// <param name="SelectedCustomer"></param>
        //public CreateEditCustomerViewModel(CustomerVM SelectedCustomer)
        //{
        //    Init();
        //    //MessageBox.Show("Entered constructor with selected customer");
        //    Customer = SelectedCustomer;
        //    IsEdit = true;
        //    // contact van deze customer
        //}

        /// <summary>
        /// Constructor for a new customer if no customer is selected
        /// </summary>
        public CreateEditCustomerViewModel(CustomerVM customer)
        {
            Init();
            // MessageBox.Show("Entered constructor with no customer existing");
            if (customer == null)
            {
                Customer = new CustomerVM();
                _Context.Customers.Add(Customer.ToModel());
            } else
            {
                //Customer = customer;
                Customer = _Context.CustomerCrud.GetCustomerById(customer.Id);
            }
            // IsEdit = false;
            // Contact aanmaken
        }

        //public CreateEditCustomerViewModel()
        //{
        //    Init();
        //    Customer = new CustomerVM();           
        //}

        ///// <summary>
        ///// Code Behind ding
        ///// </summary>
        ///// <param name="customer"></param>
        //public void SetCustomer(CustomerVM customer)
        //{
        //    Customer = customer;
        //    RaisePropertyChanged("Customer");
        //}

        /// <summary>
        /// Initializer of needed components
        /// </summary>
        public void Init()
        {
            _Context = new CustomFSContext();
            SaveChangesCommand = new RelayCommand(SaveChanges);
            DiscardCommand = new RelayCommand<Window>(Discard);
        }

        //private void AddCustomer()
        //{
        //    // not tested yet
        //    _Context.CustomerCrud.GetGetAllCustomerVMs().Add(Customer);
        //    _Context.Customers.Add(Customer.ToModel());
        //    //_Context.CustomerCrud.Add(Customer);
        //    RaisePropertyChanged();
        //}

        private void SaveChanges()
        {
            _Context.CustomerCrud.GetGetAllCustomerVMs().Add(Customer);
            _Context.SaveChanges();
            
            //if (Customer.Id != 0)
            //{
            //    ModifyCustomer();
            //} else
            //{
            //    AddCustomer();
            //}
            // not tested yet
            _Context.CustomerCrud.GetAllCustomerVMs().Add(Customer);
            _Context.CustomerCrud.Add(Customer);
        }

        /*=> _Context.CustomerCrud.Modify(Customer);*/
        // Not tested yet
        //private void ModifyCustomer()
        //{
        //    _Context.SaveChanges();
        //}


        // Not tested yet
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
