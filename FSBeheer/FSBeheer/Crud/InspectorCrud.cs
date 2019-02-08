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
    public class InspectorCrud : AbstractCrud
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
            var inspectors = CustomFSContext.Inspectors.ToList().Where(i => i.IsDeleted == false).Select(i => new InspectorVM(i));
            var availableInspectors = new List<InspectorVM>();
            foreach (InspectorVM inspectorVM in inspectors)
            {
                var scheduleItems = inspectorVM.ToModel().ScheduleItems.Select(s => new ScheduleItemVM(s));
                var scheduleItemsCollection = new ObservableCollection<ScheduleItemVM>(scheduleItems);
                if (IsAvailable(scheduleItemsCollection, dateRange))
                {
                    availableInspectors.Add(inspectorVM);
                }
            }
            return new ObservableCollection<InspectorVM>(availableInspectors);
        }

        private bool IsAvailable(ObservableCollection<ScheduleItemVM> scheduleItems, List<DateTime> dateRange)
        {
            foreach (ScheduleItemVM scheduleItem in scheduleItems)
            {
                if (scheduleItem.Date >= dateRange[0] && scheduleItem.Date <= dateRange[1])
                {
                    return false;
                }
            }
            return true;
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

        public ObservableCollection<InspectorVM> GetInspectorsByInspectionId(int inspectionId)
        {
            var inspection = new InspectionCrud(CustomFSContext).GetInspectionById(inspectionId);
            return inspection.Inspectors;
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
