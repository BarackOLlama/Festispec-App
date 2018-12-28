using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.Crud
{
    public class StatusCrud : AbstractCrud
    {
        public StatusCrud(CustomFSContext customFSContext) : base(customFSContext)
        {

        }

        public ObservableCollection<StatusVM> GetAllStatusVMs()
        {
            var status = CustomFSContext.Statuses
               .ToList()
               .Select(s => new StatusVM(s));
            var _statuses = new ObservableCollection<StatusVM>(status);

            return _statuses;
        }

        public StatusVM GetStatusById(int statusId)
        {
            var status = CustomFSContext.Statuses
                .ToList()
                .FirstOrDefault(s => s.Id == statusId);
            return new StatusVM(status);
        }

    }
}
