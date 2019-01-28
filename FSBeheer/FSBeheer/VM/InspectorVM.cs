using FSBeheer.API;
using FSBeheer.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;

namespace FSBeheer.VM
{
    public class InspectorVM : ViewModelBase
    {
        private Inspector _inspector;

        public InspectorVM(Inspector inspector)
        {
            _inspector = inspector;
        }

        public InspectorVM()
        {
            _inspector = new Inspector();
        }

        internal Inspector ToModel()
        {
            return _inspector;
        }

        public int Id
        {
            get { return _inspector.Id; }
        }

        public string Name
        {
            get { return _inspector.Name; }
            set { _inspector.Name = value; }
        }

        public string Address
        {
            get { return _inspector.Address; }
            set { _inspector.Address = value; }
        }

        public string City
        {
            get { return _inspector.City; }
            set { _inspector.City = value; }
        }

        public string Zipcode
        {
            get { return _inspector.Zipcode; }
            set { _inspector.Zipcode = value; }
        }

        public string Email
        {
            get { return _inspector.Email; }
            set { _inspector.Email = value; }
        }

        public string PhoneNumber
        {
            get { return _inspector.PhoneNumber; }
            set { _inspector.PhoneNumber = value; }
        }

        public DateTime? CertificationDate
        {
            get { return _inspector.CertificationDate; }
            set { _inspector.CertificationDate = value; }
        }

        public DateTime? InvalidDate
        {
            get { return _inspector.InvalidDate; }
            set { _inspector.InvalidDate = value; }
        }

        public string BankNumber
        {
            get { return _inspector.BankNumber; }
            set { _inspector.BankNumber = value; }
        }

        public int? AccountId
        {
            get { return _inspector.AccountId; }
        }

        public ObservableCollection<Inspection> Inspection
        {
            get { return _inspector.Inspections; }
        }

        public ObservableCollection<Inspection> RecentInspection
        {
            get { return _inspector.Inspections; }
        }

        public string TravelDistance { get; set; }

        public void SetTravelDistance(string addressTo)
        {
            using (var geo = new Geodan())
            {
                this.TravelDistance = geo.FindRoute(_inspector.Address, addressTo);
            }
        }

        public bool IsDeleted
        {
            get { return _inspector.IsDeleted; }
            set { _inspector.IsDeleted = value; RaisePropertyChanged(nameof(IsDeleted)); }
        }
    }
}
