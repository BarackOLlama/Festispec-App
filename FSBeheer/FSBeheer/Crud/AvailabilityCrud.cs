using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Crud
{
    class AvailabilityCrud : AbstractCrud
    {

        public AvailabilityCrud(CustomFSContext customFSContext) : base(customFSContext)
        {

        }

        public ObservableCollection<AvailabilityVM> GetAvailabilities()
        {
            var availability = CustomFSContext.Availabilities
                .ToList()
                .Select(i => new AvailabilityVM(i));
            var _inspections = new ObservableCollection<AvailabilityVM>(availability);
            return _inspections;
        }

    }
}
