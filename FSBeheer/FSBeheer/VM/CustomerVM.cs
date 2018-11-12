using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBeheer.Model;

namespace FSBeheer.VM
{
    public class CustomerVM : ViewModelBase
    {
        private Customer _customer; 

        public CustomerVM()
        {
            _customer = new Customer();
        }

        public CustomerVM(Customer customer)
        {
            this._customer = customer;
        }

        // Used through the whole assembly to adjust the values of a particular customer
        internal Customer ToModel()
        {
            return _customer;
        }

        public void AddCustomer()
        {
            //CustomFSContext.Customer.add(_customer);
        }

        // Properties of Quiz (Database)
        public int Id
        {
            get { return _customer.Id; }
        }

        public string Name
        {
            get { return _customer.Name; }
        }

        // TODO - rest of the properties
    }
}
