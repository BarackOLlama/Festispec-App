using FSBeheer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.VM
{
    public class EventVM
    {
        private Event _event;

        public EventVM(Event e)
        {
            _event = e;
        }

        public int Id
        {
            get { return _event.Id; }
        }

        public string Name
        {
            get { return _event.Name; }
        }

        public string Address
        {
            get { return _event.Address; }
        }

        public string City
        {
            get { return _event.City; }
        }

        public string CustomerName
        {
            get { return _event.Customer.Name; }
        }

        internal Event ToModel()
        {
            return _event;
        }
    }
}
