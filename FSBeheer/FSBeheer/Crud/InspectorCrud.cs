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
    class InspectorCrud : AbstractCrud
    {

        public InspectorCrud(CustomFSContext customFSContext) : base(customFSContext)
        {

        }

        public ObservableCollection<InspectorVM> GetAllInspectors()
        {
            var inspectors = CustomFSContext.Inspectors
                .ToList()
                .Where(i => i.IsDeleted == false)
                .Select(i => new InspectorVM(i));
            return new ObservableCollection<InspectorVM>(inspectors);
        }

        public InspectorVM GetInspectorById(int inspectorId)
        {
            var inspector = CustomFSContext.Inspectors.FirstOrDefault(i => i.Id == inspectorId);
            return new InspectorVM(inspector);
        }

        public ObservableCollection<InspectorVM> GetAllInspectorsFiltered(string must_contain)
        {
            if (string.IsNullOrEmpty(must_contain))
            {
                throw new ArgumentNullException(nameof(must_contain));
            }

            must_contain = must_contain.ToLower();

            var inspectors = CustomFSContext.Inspectors
                .ToList()
                .Where(i => i.IsDeleted == false)
                .Where(i =>
                i.Id.ToString().ToLower().Contains(must_contain) ||
                i.Name.ToLower().Contains(must_contain) ||
                i.Address.ToLower().Contains(must_contain) ||
                i.City.ToLower().Contains(must_contain)
                ).Distinct()
                .Select(i => new InspectorVM(i));
            var _inspectors = new ObservableCollection<InspectorVM>(inspectors);
            return _inspectors;
        }

        public ObservableCollection<InspectorVM> GetAllInspectorsFilteredByAvailability(List<DateTime> dateRange) //Startdate and enddate of the inspection
        {
            var inspectors = CustomFSContext.Inspectors
                .ToList()
                .Where(i => IsAvailable(
                    new ObservableCollection<AvailabilityVM>(
                        i.Availabilities.ToList()
                        .Select(a => new AvailabilityVM(a))
                    ),
                    dateRange))
                .Select(i => new InspectorVM(i));
            return new ObservableCollection<InspectorVM>(inspectors);
        }

        private bool IsAvailable(ObservableCollection<AvailabilityVM> availabilities, List<DateTime> dateRange)
        {
            List<bool> availableList = new List<bool>();
            foreach(AvailabilityVM availability in availabilities)
            {
                if(availability.Date > dateRange[0] && availability.Date < dateRange[1])
                {
                    if(availability.Scheduled == false)
                    {
                        availableList.Add(true);
                    }
                    else
                    {
                        availableList.Add(false);
                    }
                }
            }

            if(availableList.Count == 0)
            {
                return false;
            }
            return !availableList.Contains(false);
        }

        public void Delete(InspectorVM _inspector)
        {
            CustomFSContext.Inspectors.Attach(_inspector?.ToModel());
            CustomFSContext.Inspectors.Remove(_inspector?.ToModel());
            CustomFSContext.SaveChanges();
        }
    }
}
