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

        public ObservableCollection<ScheduleItemVM> GetAllScheduleItemsByInspector(int inspectorId)
        {
            var scheduleitems = CustomFSContext.ScheduleItems
                .ToList()
                .Where(s => s.IsDeleted == false)
                .Where(s => s.Inspector.Id == inspectorId)
                .Select(i => new ScheduleItemVM(i));
            var _scheduleitems = new ObservableCollection<AvailabilityVM>(scheduleitems);
            return _scheduleitems;
        }

        public ObservableCollection<ScheduleItemVM> GetScheduled()
        {
            var scheduleitem = CustomFSContext.ScheduleItems
                .ToList()
                .Where(s => s.IsDeleted == false)
                .Where(s => s.Scheduled == false)
                .Select(i => new ScheduleItemVM(i));
            var _scheduleitem = new ObservableCollection<ScheduleItemVM>(scheduleitem);
            return _scheduleitem;
        }

        public ObservableCollection<ScheduleItemVM> GetNonScheduled()
        {
            var scheduleitem = CustomFSContext.ScheduleItems
                .ToList()
                .Where(s => s.IsDeleted == false)
                .Where(s => s.Scheduled == true)
                .Select(i => new ScheduleItemVM(i));
            var _scheduleitem = new ObservableCollection<ScheduleItemVM>(scheduleitem);
            return _scheduleitem;
        }

        public void RemoveScheduleItemByInspectorList(ObservableCollection<InspectorVM> inspectorList, InspectionVM inspection)
        {
            var startRemoveDate = inspection.InspectionDate.StartDate;
            var endRemoveDate = inspection.InspectionDate.EndDate;
            var allScheduleItemVMs = CustomFSContext.ScheduleItems.Select(a => new ScheduleItemVM(a));

            foreach (InspectorVM inspectorVM in inspectorList)
            {
                foreach (ScheduleItemVM scheduleItemVM in allScheduleItemVMs)
                {
                    if (scheduleItemVM.Date >= startRemoveDate && scheduleItemVM.Date <= endRemoveDate && scheduleItemVM.Inspector.Id == inspectorVM.Id)
                        CustomFSContext.ScheduleItems.Remove(scheduleItemVM.ToModel());
                }
            }
        }
    }
}
