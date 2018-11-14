using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;

namespace FSBeheer.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private CustomFSContext _Context;

        public RelayCommand OpenCustomerManagement { get; set; }

        public RelayCommand ShowCustomersViewCommand { get; set; }
        public RelayCommand ShowInspectionViewCommand { get; set; }
        public RelayCommand ShowEventViewCommand { get; set; }
        public RelayCommand ShowInspectorsCommand { get; set; }
        public RelayCommand ShowQuotationsCommand { get; set; }
        public RelayCommand ShowQuestionnairesCommand { get; set; }


        public HomeViewModel()
        {
            _Context = new CustomFSContext();

            ShowCustomersViewCommand = new RelayCommand(ShowCustomersView);
            ShowInspectionViewCommand = new RelayCommand(ShowInspectionsView);
            ShowEventViewCommand = new RelayCommand(ShowEventsView);
            ShowInspectorsCommand = new RelayCommand(ShowInspectorsView);
            ShowQuotationsCommand = new RelayCommand(ShowQuotationsView);
            ShowQuestionnairesCommand = new RelayCommand(ShowQuestionnairesView);

            // Tests to make sure everything is working
            _Context = new CustomFSContext();
            ObservableCollection<CustomerVM> test = _Context.CustomerCrud.GetGetAllCustomerVMs();
            ObservableCollection<CustomerVM> test2 = _Context.CustomerCrud.GetFilteredCustomerBasedOnName("F");
            ObservableCollection<CustomerVM> test3 = _Context.CustomerCrud.GetCustomerById(51);

            // Place brakepoint here
            Console.WriteLine("");
        }

        private void ShowCustomersView()
        {
            new CustomerManagementView().Show();
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