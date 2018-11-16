using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Crud
{
    class InspectionCrud
    {

        private CustomFSContext _customFSContext;

        public InspectionCrud(CustomFSContext customFSContext)
        {
            _customFSContext = customFSContext;
        }

        public ObservableCollection<InspectionVM> GetInspectionVMs => _getInspections();

        private ObservableCollection<InspectionVM> _getInspections()
        {
            var _inspection = _customFSContext.Inspections
                .ToList()
                .Select(i => new InspectionVM(i));
            var _inspections = new ObservableCollection<InspectionVM>(_inspection);

            return _inspections;
        }

        //private ObservableCollection<CustomerVM> _getMultipleInspectionsByName(string name_contains)
        //{
        //    if (name_contains == null)
        //    {
        //        throw new ArgumentNullException(nameof(name_contains));
        //    }

        //    var customer = CustomFSContext.Inspections
        //       .ToList()
        //       .Where(c => c.Name.Contains(name_contains))
        //       .Select(c => new CustomerVM(c));
        //    var _customers = new ObservableCollection<CustomerVM>(customer);

        //    return _customers;
        //}

        ///*
        // * Returns one customer based on ID
        // */
        //public ObservableCollection<CustomerVM> GetCustomerById(int customer_id)
        //{
        //    var customer = CustomFSContext.Customers
        //       .ToList()
        //       .Where(c => c.Id == customer_id)
        //       .Select(c => new CustomerVM(c));
        //    var _customers = new ObservableCollection<CustomerVM>(customer);

        //    return _customers;
        //}

        //public void Add(CustomerVM _customer) => CustomFSContext.Customers.Add(_customer.ToModel());

        //public void Modify(CustomerVM _customer)
        //{
        //    // SelectedCustomer
        //    CustomFSContext.Entry(_customer?.ToModel()).State = EntityState.Modified;
        //    CustomFSContext.SaveChanges();
        //}

        //public void Delete(CustomerVM _customer)
        //{
        //    CustomFSContext.Customers.Attach(_customer?.ToModel());
        //    CustomFSContext.Customers.Remove(_customer?.ToModel());
        //    CustomFSContext.SaveChanges();
        //}

    }
}
