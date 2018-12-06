using FSBeheer.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.VM
{
    class QuotationVM : ViewModelBase
    {
        private Quotation _Quotation;

        public QuotationVM(Quotation quotation)
        {
            _Quotation = quotation;
        }

        public int Id
        {
            get { return _Quotation.Id; }
        }

        public decimal? Price
        {
            get { return _Quotation.Price; }
        }

        public DateTime? Date
        {
            get { return _Quotation.Date; }
        }

        public string Description
        {
            get { return _Quotation.Description; }
        }

        public int? CustomerId
        {
            get { return _Quotation.CustomerId; }  
        }

        public CustomerVM Customer
        {
            get { return new CustomerVM(_Quotation.Customer); }
        }

        public int? InspectionId
        {
            get { return _Quotation.InspectionId; }
        }

        public InspectionVM Inspection
        {
            get { return new InspectionVM(_Quotation.Inspection); }
        }

        public bool IsDeleted
        {
            get { return _Quotation.IsDeleted; }
        }

        public Quotation ToModel
        {
            get { return _Quotation; }
        }
    }
}
