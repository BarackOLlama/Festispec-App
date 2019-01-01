using FSBeheer.VM;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace FSBeheer.Crud
{
    public class CustomerCrud : AbstractCrud
    {
        public ObservableCollection<CustomerVM> GetFilteredCustomerBasedOnName(string name_contains) => _getMultipleCustomersByName(name_contains);
        public CustomerCrud(CustomFSContext customFSContext) : base(customFSContext)
        {
        }


        /*
         * Returns all customers
         */

        public ObservableCollection<CustomerVM> GetAllCustomers()
        {
            var customer = CustomFSContext.Customers
               .ToList()
               .Where(c => c.IsDeleted == false)
               .Select(c => new CustomerVM(c));
            var _customers = new ObservableCollection<CustomerVM>(customer);

            return _customers;
        }

        /*
         * TODO: Not need anymore?
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

        public ObservableCollection<CustomerVM> GetAllCustomersFiltered(string must_contain)
        {
            if (string.IsNullOrEmpty(must_contain))
            {
                throw new ArgumentNullException(nameof(must_contain));
            }

            must_contain = must_contain.ToLower();

            var customers = CustomFSContext.Customers
                .ToList()
                .Where(i => i.IsDeleted == false)
                .Where(i =>
                i.Id.ToString().ToLower().Contains(must_contain) ||
                i.Name != null && i.Name.ToLower().Contains(must_contain) ||
                i.Address != null && i.Address.ToLower().Contains(must_contain) ||
                i.City != null && i.City.ToLower().Contains(must_contain) ||
                (i.StartingDate != null && i.StartingDate.ToString().ToLower().Contains(must_contain))
                ).Distinct()
                .Select(i => new CustomerVM(i));
            var _customers = new ObservableCollection<CustomerVM>(customers);
            return _customers;
        }
    }
}
