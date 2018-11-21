using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Crud
{
    class InspectionCrud : AbstractCrud
    {

        public InspectionCrud(CustomFSContext customFSContext) : base(customFSContext)
        {
        }

        public ObservableCollection<InspectionVM> GetInspections()
        {
            var inspection = CustomFSContext.Inspections
                .ToList()
                .Select(i => new InspectionVM(i));
            var _inspections = new ObservableCollection<InspectionVM>(inspection);
            return _inspections;
        }



    }
}
