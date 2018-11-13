using FSBeheer.Model;
using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Crud
{
    class CustomerCrud
    {
        private CustomFSContext _customFSContext;

        public ObservableCollection<CustomerVM> GetCustomerVMs => _getCustomers();
        public CustomerCrud(CustomFSContext customFSContext)
        {
            _customFSContext = customFSContext;
        }

        public ObservableCollection<CustomerVM> _getCustomers()
        {
            var customer = _customFSContext.Customers
               .ToList()
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
