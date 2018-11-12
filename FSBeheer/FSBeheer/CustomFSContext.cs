﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using FSBeheer.VM;
using System.Threading.Tasks;
using FSBeheer.Crud;
using System.Data.Entity;

namespace FSBeheer
{
    class CustomFSContext: FSContext
    {
        public CustomerCrud CustomerCrud;

        public CustomFSContext() : base() {

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CustomFSContext, Migrations.Configuration>());

            CustomerCrud = new CustomerCrud(this);
        }

        public ObservableCollection<CustomerVM> GetCustomers()
        {
            var customer = Customers
               .ToList()
               .Select(c => new CustomerVM(c));
            var _customers = new ObservableCollection<CustomerVM>(customer);

            return _customers;
        }

    }
}
