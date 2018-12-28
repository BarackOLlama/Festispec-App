using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Crud
{
    public class InspectionDateCrud : AbstractCrud
    {

        public InspectionDateCrud(CustomFSContext customFSContext) : base(customFSContext)
        {

        }

        public ObservableCollection<InspectionDateVM> GetInspectionDates()
        {
            var inspectionDate = CustomFSContext.InspectionDates
                .ToList()
                .Select(i => new InspectionDateVM(i));
            var _inspectionDate = new ObservableCollection<InspectionDateVM>(inspectionDate);
            return _inspectionDate;
        }

    }
}
