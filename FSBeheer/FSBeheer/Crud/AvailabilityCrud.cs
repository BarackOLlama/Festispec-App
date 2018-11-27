using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        // TODO waarom zijn ze allemaal true op Scheduled?
        public ObservableCollection<AvailabilityVM> GetAvailabilities()
        {
            var availability = CustomFSContext.Availabilities
                .ToList()
                .Where(s => s.Scheduled = false)
                .Select(i => new AvailabilityVM(i));
            var _availability = new ObservableCollection<AvailabilityVM>(availability);
            return _availability;
        }

        public ObservableCollection<AvailabilityVM> GetUnavailable()
        {
            var availability = CustomFSContext.Availabilities
                .ToList()
                .Where(s => s.Scheduled = true)
                .Select(i => new AvailabilityVM(i));
            var _availability = new ObservableCollection<AvailabilityVM>(availability);
            return _availability;
        }

    }
}
