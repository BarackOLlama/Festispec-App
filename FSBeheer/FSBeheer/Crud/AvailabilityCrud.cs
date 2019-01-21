﻿using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Crud
{
    public class AvailabilityCrud : AbstractCrud
    {

        public AvailabilityCrud(CustomFSContext customFSContext) : base(customFSContext)
        {

        }

        public ObservableCollection<AvailabilityVM> GetAllAvailabilitiesByInspector(int inspectorId)
        {
            var availabilities = CustomFSContext.Availabilities
                .ToList()
                .Where(s => s.IsDeleted == false)
                .Where(s => s.Inspector.Id == inspectorId)
                .Select(i => new AvailabilityVM(i));
            var _availabilities = new ObservableCollection<AvailabilityVM>(availabilities);
            return _availabilities;
        }

        public ObservableCollection<AvailabilityVM> GetAvailabilities()
        {
            var availability = CustomFSContext.Availabilities
                .ToList()
                .Where(s => s.IsDeleted == false)
                .Where(s => s.Scheduled == false)
                .Select(i => new AvailabilityVM(i));
            var _availability = new ObservableCollection<AvailabilityVM>(availability);
            return _availability;
        }

        public ObservableCollection<AvailabilityVM> GetUnavailable()
        {
            var availability = CustomFSContext.Availabilities
                .ToList()
                .Where(s => s.IsDeleted == false)
                .Where(s => s.Scheduled == true)
                .Select(i => new AvailabilityVM(i));
            var _availability = new ObservableCollection<AvailabilityVM>(availability);
            return _availability;
        }

        public void RemoveAvailabilitiesByInspectorList(ObservableCollection<InspectorVM> inspectorList, InspectionVM inspection)
        {
            var startRemoveDate = inspection.InspectionDate.StartDate;
            var endRemoveDate = inspection.InspectionDate.EndDate;
            var allAvailabilityVMs = CustomFSContext.Availabilities.Select(a => new AvailabilityVM(a));

            foreach (InspectorVM inspectorVM in inspectorList)
            {
                foreach (AvailabilityVM availabilityVM in allAvailabilityVMs)
                {
                    if (availabilityVM.Date >= startRemoveDate && availabilityVM.Date <= endRemoveDate && availabilityVM.Inspector.Id == inspectorVM.Id)
                        CustomFSContext.Availabilities.Remove(availabilityVM.ToModel());
                }
            }
        }

    }
}
