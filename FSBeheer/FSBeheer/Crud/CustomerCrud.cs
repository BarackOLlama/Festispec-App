using FSBeheer.VM;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace FSBeheer.Crud
{
    class CustomerCrud : AbstractCrud
    {
        public ObservableCollection<CustomerVM> GetFilteredCustomerBasedOnName(string name_contains) => _getMultipleCustomersByName(name_contains);
        public CustomerCrud(CustomFSContext customFSContext) : base(customFSContext)
        {
        }


        /*
         * Returns all customers
         */

        public ObservableCollection<CustomerVM> GetAllCustomerVMs()
        {
            var customer = CustomFSContext.Customers
               .ToList()
               .Where(c => c.IsDeleted == false)
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

            var customer = CustomFSContext.Customers
               .ToList()
               .Where(c => c.Name.Contains(name_contains))
               .Select(c => new CustomerVM(c));
            var _customers = new ObservableCollection<CustomerVM>(customer);

            return _customers;
        }

        /*
         * Returns one customer based on ID
         */
        public CustomerVM GetCustomerById(int customer_id)
        {
            return new CustomerVM(CustomFSContext.Customers
               .ToList()
               .FirstOrDefault(c => c.Id == customer_id));
        }

        public void SetDeleted(CustomerVM _customer)
        {
            // isDeleted is true veld check zo ja dan krijg je alle deleted terug!
        }
    }
}
