using FSBeheer.Model;
using GalaSoft.MvvmLight;

namespace FSBeheer.VM
{
    public class EventVM : ViewModelBase
    {
        private Event _event;

        public EventVM(Event e)
        {
            _event = e;
        }

        public EventVM()
        {
            _event = new Event();
            EventDate = new EventDateVM();
        }

        public int Id
        {
            get { return _event.Id; }
        }

        public string Name
        {
            get { return _event.Name; }
            set
            {
                _event.Name = value;
                base.RaisePropertyChanged(nameof(Name));
            }
        }

        public string Address
        {
            get { return _event.Address; }
            set
            {
                _event.Address = value;
                base.RaisePropertyChanged(nameof(Address));
            }
        }

        public string City
        {
            get { return _event.City; }
            set
            {
                _event.City = value;
                base.RaisePropertyChanged(nameof(City));
            }
        }

        public string Zipcode
        {
            get { return _event.Zipcode; }
            set {
                _event.Zipcode = value;
                base.RaisePropertyChanged(nameof(Zipcode));
            }
        }

        public EventDateVM EventDate
        {
            get { return new EventDateVM(_event.EventDate); }
            set
            {
                if (value != null)
                    _event.EventDate = value.ToModel();
                base.RaisePropertyChanged(nameof(EventDate));
            }
        }

        public CustomerVM Customer
        {
            get { return new CustomerVM(_event.Customer); }
            set
            {
                if (value != null)
                    _event.Customer = value.ToModel();
                base.RaisePropertyChanged(nameof(Customer));
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
