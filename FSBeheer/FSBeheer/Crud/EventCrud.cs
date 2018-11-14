using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBeheer.VM;

namespace FSBeheer.Crud
{
    class EventCrud : AbstractCrud
    {
        public EventCrud(CustomFSContext customFSContext) : base(customFSContext)
        {

        }
        public ObservableCollection<EventVM> GetAllEventVMs()
        {
            var events = CustomFSContext.Events
               .ToList()
               .Select(c => new EventVM(c));
            var _events = new ObservableCollection<EventVM>(events);

            return _events;
        }

        public ObservableCollection<EventVM> GetFilteredEventsByString(string must_contain)
        {
            if (must_contain == null)
            {
                throw new ArgumentNullException(nameof(must_contain));
            }
            var events = CustomFSContext.Events
                .ToList()
                .Where(c =>
                c.Name.Contains(must_contain) ||
                c.City.Contains(must_contain) ||
                c.Customer.Name.Contains(must_contain) ||
                c.Address.Contains(must_contain) ||
                c.Id.Equals(must_contain)
                )

                .Select(c => new EventVM(c));
            var _events = new ObservableCollection<EventVM>(events);

            return _events;

        }
    }
}
