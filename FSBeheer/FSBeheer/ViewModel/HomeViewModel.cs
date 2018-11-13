using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using FSBeheer.View;

namespace FSBeheer.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private FSContext _context;

        public RelayCommand OpenCustomerManagement { get; set; }

        public HomeViewModel()
        {
            _context = new FSContext();
            OpenCustomerManagement = new RelayCommand(OpenCustomer);
        }

        private void OpenCustomer()
        {
            new CustomerManagementView().Show();
        }
    }
}