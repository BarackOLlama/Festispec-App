using FSBeheer.View;
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

        private CustomerListWindow _customerListWindow;

        public RelayCommand ShowCustomersViewCommand { get; set; }
        public RelayCommand ShowInspectionViewCommand { get; set; }
        public RelayCommand ShowEventViewCommand { get; set; }
        public RelayCommand ShowInspectorsCommand { get; set; }
        public RelayCommand ShowQuotationsCommand { get; set; }
        public RelayCommand ShowQuestionnairesCommand { get; set; }


        public HomeViewModel()
        {
            _context = new FSContext();
            OpenCustomerManagement = new RelayCommand(OpenCustomer);
        }

            ShowCustomersViewCommand = new RelayCommand(ShowCustomersView);
            ShowInspectionViewCommand = new RelayCommand(ShowInspectionsView);
            ShowEventViewCommand = new RelayCommand(ShowEventsView);
            ShowInspectorsCommand = new RelayCommand(ShowInspectorsView);
            ShowQuotationsCommand = new RelayCommand(ShowQuotationsView);
            ShowQuestionnairesCommand = new RelayCommand(ShowQuestionnairesView);


            var context = new CustomFSContext();
            ObservableCollection<CustomerVM> test = context.CustomerCrud.GetCustomerVMs;
        }

        private void ShowCustomersView()
        {
            _customerListWindow = new CustomerListWindow();
            _customerListWindow.Show();
        }

        private void ShowInspectionsView()
        {
            throw new NotImplementedException();
        }

        private void ShowEventsView()
        {
            throw new NotImplementedException();
        }

        private void ShowInspectorsView()
        {
            throw new NotImplementedException();
        }

        private void ShowQuotationsView()
        {
            throw new NotImplementedException();
        }

        private void ShowQuestionnairesView()
        {
            throw new NotImplementedException();
        }

    }
}