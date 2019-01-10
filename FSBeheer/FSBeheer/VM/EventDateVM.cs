using FSBeheer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.VM
{
    public class EventDateVM
    {
        private EventDate _eventDate;

        public EventDateVM(EventDate ed)
        {
            if (ed != null) {
                _eventDate = ed;
            } else
            {
                _eventDate = new EventDate();
            }
            
        }

        public int Id
        {
            get { return _eventDate.Id; }
        }

        public DateTime? StartDate
        {
            get { return _eventDate.StartDate; }
            set { _eventDate.StartDate = value; }
        }

        public DateTime? EndDate
        {
            get { return _eventDate.EndDate; }
            set { _eventDate.EndDate = value; }
        }

        public bool IsDeleted
        {
            get { return _eventDate.IsDeleted; }
            set { _eventDate.IsDeleted = value; }
        }

        internal EventDate ToModel()
        {
            return _eventDate;
        }
    }
}
