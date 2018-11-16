using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Crud
{
    class InspectorCrud
    {

        private CustomFSContext _customFSContext;

        public ObservableCollection<CustomerVM> GetCustomerVMs => _getCustomers();
        public InspectorCrud(CustomFSContext customFSContext)
        {
            _customFSContext = customFSContext;
        }

        private ObservableCollection<CustomerVM> _getCustomers()
        {
            var inspector = _customFSContext.Customers
               .ToList()
               .Select(c => new CustomerVM(c));
            var _customers = new ObservableCollection<CustomerVM>(inspector);

            return _customers;
        }

    }
}
