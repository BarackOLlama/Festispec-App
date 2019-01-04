using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using FSBeheer.VM;

namespace FSBeheer.Crud
{
    public class EventCrud : AbstractCrud
    {
        public EventCrud(CustomFSContext customFSContext) : base(customFSContext)
        {

        }
        public ObservableCollection<EventVM> GetAllEvents()
        {
            var events = CustomFSContext.Events
               .ToList()
               .Where(e => e.IsDeleted == false)
               .Select(e => new EventVM(e));
            var _events = new ObservableCollection<EventVM>(events);

            return _events;
        }

        public ObservableCollection<EventVM> GetAllEventsFiltered(string must_contain)
        {
            if (string.IsNullOrEmpty(must_contain))
            {
                throw new ArgumentNullException(nameof(must_contain));
            }

            must_contain = must_contain.ToLower();

            var events = CustomFSContext.Events
                .ToList()
                .Where(c => c.IsDeleted == false)
                .Where(e =>
                e.Id.ToString().ToLower().Contains(must_contain) ||
                e.Name.ToLower().Contains(must_contain) ||
                e.City.ToLower().Contains(must_contain) ||
                e.Customer.Name.ToLower().Contains(must_contain) ||
                e.Address.ToLower().Contains(must_contain)
                ).Distinct()
                .Select(e => new EventVM(e));
            return new ObservableCollection<EventVM>(events);
        }

        public EventVM GetEventById(int eventId)
        {
            var _event = CustomFSContext.Events
               .ToList()
                .Where(e => e.IsDeleted == false)
               .FirstOrDefault(e => e.Id == eventId);
            return new EventVM(_event);
        }

        public void Delete(EventVM _event)
        {
            _event.SetDeleted();
            CustomFSContext.SaveChanges();
        }
    }
}
