using FSBeheer.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.VM
{
    public class InspectionVM : ViewModelBase
    {
        private Inspection _inspection;

        public InspectionVM(Inspection inspection)
        {
            _inspection = inspection;
        }

        // useful?
        internal Inspection ToModel()
        {
            return _inspection;
        }

        public int Id
        {
            get { return _inspection.Id; }
        }

        public string Name
        {
            get { return _inspection.Name; }
        }

        public string State
        {
            get { return _inspection.State; }
        }

        public string Notes
        {
            get { return _inspection.Notes; }
        }

    }
}
