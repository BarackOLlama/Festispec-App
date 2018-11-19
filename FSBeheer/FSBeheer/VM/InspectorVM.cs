using FSBeheer.Model;
using System;

namespace FSBeheer.VM
{
    public class InspectorVM
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
        }

        public string Address
        {
            get { return _inspector.Address; }
        }

        public string City
        {
            get { return _inspector.City; }
        }

        public string AddressAndCity
        {
            get { return _inspector.Address + " " + _inspector.City; }
        }

        public string Zipcode
        {
            get { return _inspector.Zipcode; }
        }

        public string Email
        {
            get { return _inspector.Email; }
        }

        public string PhoneNumber
        {
            get { return _inspector.PhoneNumber; }
        }

        public DateTime? CertificationDate
        {
            get { return _inspector.CertificationDate; }
        }

        public DateTime? InvalidDate
        {
            get { return _inspector.InvalidDate; }
        }

        public string BankNumber
        {
            get { return _inspector.BankNumber; }
        }

        public int? AccountId
        {
            get { return _inspector.AccountId; }
        }

    }
}
