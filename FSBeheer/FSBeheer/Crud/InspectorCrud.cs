using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Crud
{
    class InspectorCrud : AbstractCrud
    {

        public InspectorCrud(CustomFSContext customFSContext) : base(customFSContext)
        {
        }

        public ObservableCollection<CustomerVM> GetCustomerVMs => _getCustomers();

        private ObservableCollection<CustomerVM> _getCustomers()
        {
            var inspector = CustomFSContext.Customers
               .ToList()
               .Select(c => new CustomerVM(c));
            var _customers = new ObservableCollection<CustomerVM>(inspector);

            return _customers;
        }

    }
}
