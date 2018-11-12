using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using FSBeheer.VM;
using System.Threading.Tasks;
using FSBeheer.Crud;

namespace FSBeheer
{
    class CustomFSContext: FSContext
    {
        public CustomerCrud CustomerCrud;

        public CustomFSContext() : base() {
            CustomerCrud = new CustomerCrud(this);

        }

    }
}
