using FSBeheer.VM;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace FSBeheer.Crud
{
    class CustomerCrud
    {
        private CustomFSContext _customFSContext;

        public ObservableCollection<CustomerVM> getFilteredCustomerBasedOnName(string name_contains)
        {
            return _getMultipleCustomersByName(name_contains);
        }
        public CustomerCrud(CustomFSContext customFSContext)
        {
            _customFSContext = customFSContext;
        }

        public ObservableCollection<CustomerVM> GetAllCustomerVMs
        {
            get
            {
                var customer = _customFSContext.Customers
                   .ToList()
                   .Select(c => new CustomerVM(c));
                var _customers = new ObservableCollection<CustomerVM>(customer);

                return _customers;
            }
        }

        /**
         * Filter customer results based on a part of their name
         * */
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

        /**
         * Returns one customer based on ID
         **/
        private ObservableCollection<CustomerVM> GetCustomerById(int customer_id)
        {
            var customer = _customFSContext.Customers
               .ToList()
               .Where(c => c.Id == customer_id)
               .Select(c => new CustomerVM(c));
            var _customers = new ObservableCollection<CustomerVM>(customer);

            return _customers;
        }
    }
}
