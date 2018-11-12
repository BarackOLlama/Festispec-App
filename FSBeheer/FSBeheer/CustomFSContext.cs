using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using FSBeheer.VM;
using System.Threading.Tasks;

namespace FSBeheer
{
    class CustomFSContext: FSContext
    {
        public ObservableCollection<CustomerVM> GetCustomersVM {
            get {
                return GetCustomers();
            }
        }

        public CustomFSContext() : base() {

        }

        private ObservableCollection<CustomerVM> GetCustomers()
        {
            var customer = this.Customers
               .ToList()
               .Select(c => new CustomerVM(c));
            var _customers = new ObservableCollection<CustomerVM>(customer);

            return _customers;
        }

    }
}
