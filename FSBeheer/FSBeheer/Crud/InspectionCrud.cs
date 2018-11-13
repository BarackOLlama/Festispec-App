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
    }
}
