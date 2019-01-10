using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSBeheer.Model;

namespace FSBeheer.VM
{
    public class ContactVM : ViewModelBase
    {
        private Contact _contact;

        public ContactVM()
        {
            _contact = new Contact();
        }

        public ContactVM(Contact contact)
        {
            _contact = contact;
        }

        // Used through the whole assembly to adjust the values of a particular customer
        internal Contact ToModel()
        {
            return _contact;
        }

        // Properties of Contact (Database)
        public int Id
        {
            get { return _contact.Id; }
        }

        public string Name
        {
            get { return _contact.Name; }
            set { _contact.Name = value; RaisePropertyChanged(nameof(Name)); }
        }

        public string Email
        {
            get { return _contact.Email; }
            set { _contact.Email = value; RaisePropertyChanged(nameof(Email)); }
        }

        public string PhoneNumber
        {
            get { return _contact.PhoneNumber; }
            set { _contact.PhoneNumber = value; RaisePropertyChanged(nameof(PhoneNumber)); }
        }

        public string Note
        {
            get { return _contact.Note; }
            set { _contact.Note = value; RaisePropertyChanged(nameof(Note)); }
        }

        public CustomerVM Customer
        {
            get { return new CustomerVM(_contact.Customer); }
            set { _contact.Customer = value.ToModel(); }
        }

        public bool IsDeleted
        {
            get { return _contact.IsDeleted; }
            set { _contact.IsDeleted = value; RaisePropertyChanged(nameof(IsDeleted)); }
        }

    }
}
