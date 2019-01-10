using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Crud
{
    public class QuotationCrud : AbstractCrud
    {
        public QuotationCrud(CustomFSContext customFSContext) : base(customFSContext)
        {

        }

        public ObservableCollection<QuotationVM> GetQuotations()
        {
            var quotations = CustomFSContext.Quotations
                .ToList()
                .Where(q => q.IsDeleted == false)
                .Select(q => new QuotationVM(q));
            var _quotations = new ObservableCollection<QuotationVM>(quotations);
            return _quotations;
        }

        public void Add(QuotationVM _quotation) => CustomFSContext.Quotations.Add(_quotation.ToModel());

        public void Modify(QuotationVM _quotation)
        {
            CustomFSContext.Entry(_quotation?.ToModel()).State = EntityState.Modified;
            CustomFSContext.SaveChanges();
        }

        public void Delete(QuotationVM _quotation)
        {
            CustomFSContext.Quotations.Attach(_quotation?.ToModel());
            CustomFSContext.Quotations.Remove(_quotation?.ToModel());
            CustomFSContext.SaveChanges();
        }
    }
}
