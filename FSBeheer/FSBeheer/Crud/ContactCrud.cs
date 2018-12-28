using FSBeheer.VM;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace FSBeheer.Crud
{
    public class ContactCrud : AbstractCrud
    {
        public ObservableCollection<ContactVM> GetFilteredContactBasedOnName(string name_contains) => _getMultipleContactsByName(name_contains);
        public ContactCrud(CustomFSContext customFSContext) : base(customFSContext)
        {
        }

        public ObservableCollection<ContactVM> GetContactByCustomer(CustomerVM _customer)
        {
            var contact = CustomFSContext.Contacts
               .ToList()
               .Where(c => c.CustomerId == _customer.Id && c.IsDeleted == false)
               .Select(c => new ContactVM(c));
            var _contacts = new ObservableCollection<ContactVM>(contact);

            return _contacts;
        }

        /*
        * Returns all contacts
        */
        public ObservableCollection<ContactVM> GetAllContactVMs()
        {
            var contact = CustomFSContext.Contacts
               .ToList()
               .Select(c => new ContactVM(c));
            var _contacts = new ObservableCollection<ContactVM>(contact);

            return _contacts;
        }

        /*
         * Filter contact results based on a part of their name
         */
        private ObservableCollection<ContactVM> _getMultipleContactsByName(string name_contains)
        {
            if (name_contains == null)
            {
                throw new ArgumentNullException(nameof(name_contains));
            }

            var contact = CustomFSContext.Contacts
               .ToList()
               .Where(c => c.Name.Contains(name_contains))
               .Select(c => new ContactVM(c));
            var _contacts = new ObservableCollection<ContactVM>(contact);

            return _contacts;
        }

        /*
         * Returns one contact based on ID
         */
        public ContactVM GetContactById(int contact_id)
        {
            return new ContactVM(CustomFSContext.Contacts
               .ToList()
               .FirstOrDefault(c => c.Id == contact_id));
        }

        public void SetDeleted(ContactVM _contact)
        {
            // isDeleted is true veld check zo ja dan krijg je alle deleted terug!
        }
    }
}
