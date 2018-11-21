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

        // Properties of Quiz (Database)
        //public int Id
        //{
        //    get { return _customer.Id; }
        //}

        //public string Name
        //{
        //    get { return _customer.Name; }
        //    set { _customer.Name = value; RaisePropertyChanged("Name"); }
        //}


    }
}
