using FSBeheer.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.VM
{
    public class QuotationVM : ViewModelBase
    {
        private Quotation _quotation;

        public QuotationVM(Quotation quotation)
        {
            _quotation = quotation;
        }

        public QuotationVM()
        {
            _quotation = new Quotation();
        }

        public int Id
        {
            get { return _quotation.Id; }
        }

        internal Quotation ToModel()
        {
            return _quotation;
        }

        public decimal? Price
        {
            get { return _quotation.Price; }
        }

        public DateTime? Date
        {
            get { return _quotation.Date; }
        }

        public string Description
        {
            get { return _quotation.Description; }
        }

        public int? CustomerId
        {
            get { return _quotation.CustomerId; }
        }

        public CustomerVM Customer
        {
            get { return new CustomerVM(_quotation.Customer); }
        }

        public int? InspectionId
        {
            get { return _quotation.InspectionId; }
        }

        public InspectionVM Inspection
        {
            get { return new InspectionVM(_quotation.Inspection); }
        }

        public bool IsDeleted
        {
            get { return _quotation.IsDeleted; }
        }
    }
}
