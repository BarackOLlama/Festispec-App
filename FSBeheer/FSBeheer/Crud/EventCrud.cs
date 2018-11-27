using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using FSBeheer.VM;

namespace FSBeheer.Crud
{
    class EventCrud : AbstractCrud
    {
        public EventCrud(CustomFSContext customFSContext) : base(customFSContext)
        {

        }
        public ObservableCollection<EventVM> GetAllEvents()
        {
            var events = CustomFSContext.Events
               .ToList()
               .Select(c => new EventVM(c));
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
        //public void Add(EventVM _event) => CustomFSContext.Events.Add(_event.ToModel());

        public EventVM GetEventById(int eventId)
        {
            var customer = CustomFSContext.Events
               .ToList()
               .FirstOrDefault(e => e.Id == eventId);
            return new EventVM(customer);
        }
        //public void Modify(EventVM _event)
        //{
        //    CustomFSContext.Entry(_event?.ToModel()).State = EntityState.Modified;
        //    CustomFSContext.SaveChanges();
        //}

        public void Delete(EventVM _event)
        {
            CustomFSContext.Events.Attach(_event?.ToModel());
            CustomFSContext.Events.Remove(_event?.ToModel());
            CustomFSContext.SaveChanges();
        }
    }
}
