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
            if (string.IsNullOrEmpty(must_contain))
            {
                throw new ArgumentNullException(nameof(must_contain));
            }

            must_contain = must_contain.ToLower();

            var events = CustomFSContext.Events
                .ToList()
                .Where(c =>
                c.Name.ToLower().Contains(must_contain) ||
                c.City.ToLower().Contains(must_contain) ||
                c.Customer.Name.ToLower().Contains(must_contain) ||
                c.Address.ToLower().Contains(must_contain) ||
                c.Id.Equals(must_contain)
                ).Distinct()
                .Select(c => new EventVM(c));
            var _events = new ObservableCollection<EventVM>(events);

            return _events;

        }
        public void Add(EventVM _event) => CustomFSContext.Events.Add(_event.ToModel());

        public ObservableCollection<EventVM> GetEventById(int eventId)
        {
            var customer = CustomFSContext.Events
               .ToList()
               .Where(c => c.Id == eventId)
               .Select(c => new EventVM(c));
            var _customers = new ObservableCollection<EventVM>(customer);

            return _customers;
        }
        public void Modify(EventVM _event)
        {
            CustomFSContext.Entry(_event?.ToModel()).State = EntityState.Modified;
            CustomFSContext.SaveChanges();
        }

        public void Delete(EventVM _event)
        {
            CustomFSContext.Events.Attach(_event?.ToModel());
            CustomFSContext.Events.Remove(_event?.ToModel());
            CustomFSContext.SaveChanges();
        }
    }
}
