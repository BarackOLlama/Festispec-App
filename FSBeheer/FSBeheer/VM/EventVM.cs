using FSBeheer.Model;

namespace FSBeheer.VM
{
    public class EventVM
    {
        private Event _event;

        public EventVM(Event e)
        {
            _event = e;
        }

        public EventVM()
        {
            _event = new Event();
        }

        public int Id
        {
            get { return _event.Id; }
        }

        public string Name
        {
            get { return _event.Name; }
            set { _event.Name = value; }
        }

        public string Address
        {
            get { return _event.Address; }
            set { _event.Address = value; }
        }

        public string City
        {
            get { return _event.City; }
            set { _event.City = value; }
        }

        public string Zipcode
        {
            get { return _event.Zipcode; }
            set { _event.Zipcode = value; }
        }

        public EventDateVM EventDate
        {
            get { return new EventDateVM(_event.EventDate); }
            set
            {
                if (value != null)
                    _event.EventDate = value.ToModel();
            }
        }

        public CustomerVM Customer
        {
            get { return new CustomerVM(_event.Customer); }
            set {
                if(value != null)
                    _event.Customer = value.ToModel();
            }
        }

        public override string ToString()
        {
            return _event.Name;
        }

        internal Event ToModel()
        {
            return _event;
        }

        public void SetDeleted()
        {
            _event.IsDeleted = true;
        }
    }
}
