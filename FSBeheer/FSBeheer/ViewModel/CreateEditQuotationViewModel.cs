using FSBeheer.VM;
using GalaSoft.MvvmLight;

namespace FSBeheer.ViewModel
{
    public class CreateEditQuotationViewModel : ViewModelBase
    {
        public QuotationVM Quotation { get; set; }

        public void SetQuotation(QuotationVM quotationVM)
        {
            Quotation = quotationVM;
            // moet nog veeel uitgebreider, zie SetInspection
        }

    }
}
