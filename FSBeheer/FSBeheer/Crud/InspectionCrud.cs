using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Crud
{
    public class InspectionCrud : AbstractCrud
    {

        public InspectionCrud(CustomFSContext customFSContext) : base(customFSContext)
        {
        }

        public ObservableCollection<InspectionVM> GetAllInspectionVMs()
        {
            var inspection = CustomFSContext.Inspections
                .ToList()
                .Where(i => i.IsDeleted == false)
                .Select(i => new InspectionVM(i));
            var _inspections = new ObservableCollection<InspectionVM>(inspection);
            return _inspections;
        }

        public InspectionVM GetInspectionById(int inspectionId)
        {
            var inspection = CustomFSContext.Inspections.ToList().FirstOrDefault(i => i.Id == inspectionId);
            return new InspectionVM(inspection);
        }
    }
}
