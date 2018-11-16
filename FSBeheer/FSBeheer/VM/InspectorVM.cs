using FSBeheer.Model;
using System;

namespace FSBeheer.VM
{
    public class InspectorVM
    {

        private Inspector _Inspector;

        public InspectorVM(Inspector inspector)
        {
            _Inspector = inspector;
        }

        public int Id
        {
            get { return _Inspector.Id; }
        }

        public string Name
        {
            get { return _Inspector.Name; }
        }

        public string Address
        {
            get { return _Inspector.Address; }
        }

        public string City
        {
            get { return _Inspector.City; }
        }

        public string Zipcode
        {
            get { return _Inspector.Zipcode; }
        }

        public string Email
        {
            get { return _Inspector.Email; }
        }

        public string PhoneNumber
        {
            get { return _Inspector.PhoneNumber; }
        }

        public DateTime CertificationDate
        {
            get { return _Inspector.CertificationDate; }
        }

        public DateTime InvalidDate
        {
            get { return _Inspector.InvalidDate; }
        }

        public string BankNumber
        {
            get { return _Inspector.BankNumber; }
        }

        public int? AccountId
        {
            get { return _Inspector.AccountId; }
        }

    }
}
