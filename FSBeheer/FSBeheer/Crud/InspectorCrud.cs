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
    class InspectorCrud : AbstractCrud
    {

        public InspectorCrud(CustomFSContext customFSContext) : base(customFSContext)
        {

        }

        public ObservableCollection<InspectorVM> GetAllInspectors()
        {
            var inspectors = CustomFSContext.Inspectors
                .ToList()
                .Select(i => new InspectorVM(i));
            return new ObservableCollection<InspectorVM>(inspectors);
        }

        public ObservableCollection<InspectorVM> GetAllInspectorsFilteredByAvailability(DateTime date)
        {
            var inspectors = CustomFSContext.Inspectors
                .ToList()
                .Select(i => new InspectorVM(i));
            return new ObservableCollection<InspectorVM>(inspectors);
        }

        //public void Add(InspectorVM _inspector) => CustomFSContext.Inspectors.Add(_inspector.ToModel());

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
