using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.ViewModel
{
    class GenerateReportViewModel : ViewModelBase
    {
        private CustomFSContext _context;

        public GenerateReportViewModel()
        {
            _context = new CustomFSContext();


        }

        public void SetInspection(int inspectionId)
        {

        }
    }
}
