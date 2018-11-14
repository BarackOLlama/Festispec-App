﻿using FSBeheer.Model;
using FSBeheer.VM;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace FSBeheer.Crud
{
    class CustomerCrud
    {
        private CustomFSContext _customFSContext;

        public ObservableCollection<CustomerVM> GetFilteredCustomerBasedOnName(string name_contains) => _getMultipleCustomersByName(name_contains);
        public CustomerCrud(CustomFSContext customFSContext)
        {
            _customFSContext = customFSContext;
        }


        /*
         * Returns all customers
         */

        public ObservableCollection<CustomerVM> GetGetAllCustomerVMs()
        {
            var customer = _customFSContext.Customers
               .ToList()
               .Select(c => new CustomerVM(c));
            var _customers = new ObservableCollection<CustomerVM>(customer);

            return _customers;
        }

        /*
         * Filter customer results based on a part of their name
         */
        private ObservableCollection<CustomerVM> _getMultipleCustomersByName(string name_contains)
        {
            if (name_contains == null)
            {
                throw new ArgumentNullException(nameof(name_contains));
            }

            var customer = _customFSContext.Customers
               .ToList()
               .Where(c => c.Name.Contains(name_contains))
               .Select(c => new CustomerVM(c));
            var _customers = new ObservableCollection<CustomerVM>(customer);

            return _customers;
        }

        /*
         * Returns one customer based on ID
         */
        public ObservableCollection<CustomerVM> GetCustomerById(int customer_id)
        {
            var customer = _customFSContext.Customers
               .ToList()
               .Where(c => c.Id == customer_id)
               .Select(c => new CustomerVM(c));
            var _customers = new ObservableCollection<CustomerVM>(customer);

            return _customers;
        }

        public void Add(CustomerVM _customer) => _customFSContext.Customers.Add(_customer.ToModel());

        public void Modify(CustomerVM _customer)
        {
            // SelectedCustomer
            _customFSContext.Entry(_customer?.ToModel()).State = EntityState.Modified;
            _customFSContext.SaveChanges();
        }

        public void Delete(CustomerVM _customer)
        {
            _customFSContext.Customers.Attach(_customer?.ToModel());
            _customFSContext.Customers.Remove(_customer?.ToModel());
            _customFSContext.SaveChanges();
        }
    }
}
