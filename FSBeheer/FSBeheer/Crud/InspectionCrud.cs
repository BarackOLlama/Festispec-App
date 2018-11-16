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

        public ObservableCollection<InspectionVM> GetInspectionVMs => _getInspections();

        private ObservableCollection<InspectionVM> _getInspections()
        {
            var _inspection = CustomFSContext.Inspections
                .ToList()
                .Select(i => new InspectionVM(i));
            var _inspections = new ObservableCollection<InspectionVM>(_inspection);

            return _inspections;
        }
    }
}
