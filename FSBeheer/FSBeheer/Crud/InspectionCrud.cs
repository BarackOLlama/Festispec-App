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

        public ObservableCollection<InspectionVM> GetAllInspections()
        {
            var inspection = CustomFSContext.Inspections
                .ToList()
                .Select(i => new InspectionVM(i));
            var _inspections = new ObservableCollection<InspectionVM>(inspection);
            return _inspections;
        }

        public ObservableCollection<InspectionVM> GetAllInspectionsFiltered(string must_contain)
        {
            if (string.IsNullOrEmpty(must_contain))
            {
                throw new ArgumentNullException(nameof(must_contain));
            }

            must_contain = must_contain.ToLower();

            var inspections = CustomFSContext.Inspections
                .ToList()
                .Where(i => i.IsDeleted == false)
                .Where(i =>
                i.Id.ToString().ToLower().Contains(must_contain) ||
                i.Name.ToLower().Contains(must_contain) ||
                i.Notes.ToLower().Contains(must_contain) ||
                i.Status.ToString().ToLower().Contains(must_contain) ||
                i.Event.Name.ToLower().Contains(must_contain)
                ).Distinct()
                .Select(i => new InspectionVM(i));
            var _inspections = new ObservableCollection<InspectionVM>(inspections);
            return _inspections;
        }

        public InspectionVM GetInspectionById(int inspectionId)
        {
            var inspection = CustomFSContext.Inspections.ToList().FirstOrDefault(i => i.Id == inspectionId);
            return new InspectionVM(inspection);
        }
    }
}
