using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBeheer.Model;

namespace FSBeheer.ViewModel
{
    public class CustomerViewModel : ViewModelBase
    {
        private Customer _customer; 

        public CustomerViewModel()
        {
            _customer = new Customer();
        }

        public CustomerViewModel(Customer customer)
        {
            this._customer = customer;
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
            set { _customer.Id = value; RaisePropertyChanged("Id"); }
        }

        public string Name
        {
            get { return _customer.Name; }
            set { _customer.Name = value; RaisePropertyChanged("Name"); }
        }

        // TODO - rest of the properties
    }
}
