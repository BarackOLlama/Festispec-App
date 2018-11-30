using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBeheer.Model;

namespace FSBeheer.VM
{
    public class CustomerVM : ViewModelBase
    {
        private Customer _customer;

        public CustomerVM()
        {
            _customer = new Customer();
        }

        public CustomerVM(Customer customer)
        {
            _customer = customer;
        }

        // Used through the whole assembly to adjust the values of a particular customer
        internal Customer ToModel()
        {
            return _customer;
        }

        // Properties of Quiz (Database)
        public int Id
        {
            get { return _customer.Id; }
        }

        public string Name
        {
            get { return _customer.Name; }
            set { _customer.Name = value; RaisePropertyChanged(nameof(Name)); }
        }

        public string Adres
        {
            get { return _customer.Address; }
            set { _customer.Address = value; RaisePropertyChanged(nameof(Adres)); }
        }

        public string City
        {
            get { return _customer.City; }
            set { _customer.City = value; RaisePropertyChanged(nameof(City)); }
        }

        public string ZipCode
        {
            get { return _customer.ZipCode; }
            set { _customer.ZipCode = value; RaisePropertyChanged(nameof(ZipCode)); }
        }

        public DateTime? StartingDate
        {
            get { return _customer.StartingDate; }
            set { _customer.StartingDate = value; RaisePropertyChanged(nameof(StartingDate)); }
        }

        public decimal? ChamberOfCommerceNumber
        {
            get { return _customer.ChamberOfCommerceNumber; }
            set { _customer.ChamberOfCommerceNumber = value; RaisePropertyChanged(nameof(ChamberOfCommerceNumber)); }
        }

        public override string ToString()
        {
            return _customer.Name;
        }

        public bool IsDeleted
        {
            get { return _customer.IsDeleted; }
            set { _customer.IsDeleted = value; RaisePropertyChanged(nameof(IsDeleted)); }
        }
    }
}
