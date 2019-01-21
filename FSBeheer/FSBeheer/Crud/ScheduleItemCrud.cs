using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Crud
{
    public class ScheduleItemCrud : AbstractCrud
    {

        public ScheduleItemCrud(CustomFSContext customFSContext) : base(customFSContext)
        {

        }

        public ObservableCollection<ScheduleItemVM> GetAvailabilities()
        {
            var availability = CustomFSContext.ScheduleItems
                .ToList()
                .Where(s => s.Scheduled == false)
                .Select(i => new ScheduleItemVM(i));
            var _availability = new ObservableCollection<ScheduleItemVM>(availability);
            return _availability;
        }

        public ObservableCollection<ScheduleItemVM> GetUnavailable()
        {
            var availability = CustomFSContext.ScheduleItems
                .ToList()
                .Where(s => s.Scheduled == true)
                .Select(i => new ScheduleItemVM(i));
            var _availability = new ObservableCollection<ScheduleItemVM>(availability);
            return _availability;
        }

        public void RemoveAvailabilitiesByInspectorList(ObservableCollection<InspectorVM> inspectorList, InspectionVM inspection)
        {
            var startRemoveDate = inspection.InspectionDate.StartDate;
            var endRemoveDate = inspection.InspectionDate.EndDate;
            var allAvailabilityVMs = CustomFSContext.ScheduleItems.Select(a => new ScheduleItemVM(a));

            foreach (InspectorVM inspectorVM in inspectorList)
            {
                foreach (ScheduleItemVM availabilityVM in allAvailabilityVMs)
                {
                    if (availabilityVM.Date >= startRemoveDate && availabilityVM.Date <= endRemoveDate && availabilityVM.Inspector.Id == inspectorVM.Id)
                        CustomFSContext.ScheduleItems.Remove(availabilityVM.ToModel());
                }
            }
        }
    }
}
