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
                return true;
            }
            return !availableList.Contains(false);
        }

        public ObservableCollection<InspectorVM> GetInspectorsByList(ObservableCollection<InspectorVM> list)
        {
            var inspectors = new ObservableCollection<InspectorVM>();
            foreach (InspectorVM inspectorVM in list)
            {
                var inspector = CustomFSContext.Inspectors.FirstOrDefault(i => inspectorVM.Id == i.Id);
                inspectors.Add(new InspectorVM(inspector));
            }
            return inspectors;
        }

        public List<InspectorVM> GetInspectorsByInspectionId(int inspectionId)
        {
            var inspectors = new List<InspectorVM>();
            var inspection = new InspectionCrud(CustomFSContext).GetInspectionById(inspectionId);
            foreach (InspectorVM inspectorVM in inspection.Inspectors)
            {
                inspectors.Add(inspectorVM);
            }
            return inspectors;
        }

        public void Add(InspectorVM _inspector) => CustomFSContext.Inspectors.Add(_inspector.ToModel());

        public void Modify(InspectorVM _inspector)
        {
            CustomFSContext.Entry(_inspector?.ToModel()).State = EntityState.Modified;
            CustomFSContext.SaveChanges();
        }

        public void Delete(InspectorVM _inspector)
        {
            CustomFSContext.Inspectors.Attach(_inspector?.ToModel());
            CustomFSContext.Inspectors.Remove(_inspector?.ToModel());
            CustomFSContext.SaveChanges();
        }
    }
}
