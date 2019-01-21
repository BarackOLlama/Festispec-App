using FSBeheer.VM;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.ViewModel
{
    public class CreateEditScheduleViewModel : ViewModelBase
    {
        private CustomFSContext _Context;

        public AvailabilityVM Availability { get; set; }

        public CreateEditScheduleViewModel()
        {

        }
    }
}
