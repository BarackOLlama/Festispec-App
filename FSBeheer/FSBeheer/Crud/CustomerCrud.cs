using FSBeheer.Model;
using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public void AddCustomer(Customer _customer)
        {
            var newCustomer = new CustomerVM(new Customer
            {
                //Name = NewNameCustomer,
                //Adres = NewAdres,
                //Place = NewPlace,
                //ZipCode = NewZipCode,
                //StartingDate = NewStartingDate,
                //ChamberOfCommerceNumber = NewChamberOfCommerceNumber

            });

            //_customFSContext.Customers.Add(_customer);
        }
    }
}
