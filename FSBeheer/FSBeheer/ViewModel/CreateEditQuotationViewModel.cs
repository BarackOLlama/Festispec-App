using FSBeheer.Model;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.ViewModel
{
    class CreateEditQuotationViewModel : ViewModelBase
    {
        public QuotationVM Quotation { get; set; }

        public void SetQuotation(QuotationVM quotationVM)
        {
            Quotation = quotationVM;
            // moet nog veeel uitgebreider, zie SetInspection
        }
    }
}
