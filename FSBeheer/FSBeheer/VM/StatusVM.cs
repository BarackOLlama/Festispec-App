using FSBeheer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.VM
{
    public class StatusVM
    {
        private Status _Status;

        public StatusVM(Status status)
        {
            _Status = status;
        }

        internal Status ToModel()
        {
            return _Status;
        }

        public int Id
        {
            get { return _Status.Id; }
        }

        public string StatusName
        {
            get { return _Status.StatusName; }
        }
    }
}
