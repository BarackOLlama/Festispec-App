using FSBeheer.Model;
using FSBeheer.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBeheer.ViewModel
{
    public class CreateEditCustomerViewModel
    {
        public CustomerVM Customer { get; set; }

        // prop ContactVM

        private CustomFSContext _Context;


        // Etc...


        public CreateEditCustomerViewModel(CustomerVM c)
        {
            _Context = new CustomFSContext();

            // try catch
            if (c != null)
            {
                Customer = c;
                // contact van deze customer
            }
            else
            {
                Customer = new CustomerVM();
                _Context.CustomerCrud.Add(Customer);
                // Contact aanmaken
            }


        }
        

        // TODO: Connect to a new contact person when adding a customer 
    }
}
