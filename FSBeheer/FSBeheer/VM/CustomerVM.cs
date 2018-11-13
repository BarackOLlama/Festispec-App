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
        private CustomFSContext _customFSContext;

        public CustomerVM()
        {
            _customer = new Customer();
        }

        public CustomerVM(Customer customer)
        {
            _customer = customer;
        }

        // Used through the whole assembly to adjust the values of a particular customer
        internal Customer ToModel()
        {
            return _customer;
        }

        public void AddCustomer()
        {
            // Observable collection
            _customFSContext.GetCustomers().Add(this);

            // Database
            _customFSContext.Customers.Add(_customer);

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

        public string Adres
        {
            get { return _customer.Adres; }
        }

        public string Place
        {
            get { return _customer.Place; }
        }

        public string ZipCode
        {
            get { return _customer.ZipCode; }
        }

        public DateTime? StartingDate
        {
            get { return _customer.StartingDate; }
        }

        public short? ChamberOfCommerceNumber
        {
            get { return _customer.ChamberOfCommerceNumber; }
        }
    }
}
