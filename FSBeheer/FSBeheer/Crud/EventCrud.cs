using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
