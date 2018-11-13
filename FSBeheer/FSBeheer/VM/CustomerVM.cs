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
        private CustomFSContext _customFSContext = new CustomFSContext();

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

        // Properties of Quiz (Database)
        public int Id
        {
            get { return _customer.Id; }
        }

        public string Name
        {
            get { return _customer.Name; }
            set
            {
                _customer.Name = value;
            }
        }

        public string Adres
        {
            get { return _customer.Adres; }
            set
            {
                _customer.Adres = value;
                // RaisePropertyChanged("Adres");
            }
        }

        public string Place
        {
            get { return _customer.Place; }
            set
            {
                _customer.Place = value;
            }
        }

        public string ZipCode
        {
            get { return _customer.ZipCode; }
            set
            {
                _customer.ZipCode = value;
            }
        }

        public DateTime? StartingDate
        {
            get { return _customer.StartingDate; }
            set
            {
                _customer.StartingDate = value;
            }
        }

        public short? ChamberOfCommerceNumber
        {
            get { return _customer.ChamberOfCommerceNumber; }
            set
            {
                _customer.ChamberOfCommerceNumber = value;
            }
        }
    }
}
