using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<InspectorVM> GetInspectors()
        {
            var inspector = CustomFSContext.Inspectors
                .ToList()
                .Select(i => new InspectorVM(i));
            var _inspectors = new ObservableCollection<InspectorVM>(inspector);
            return _inspectors;
        }
    }
}
