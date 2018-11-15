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
        private CustomerManagementView _customerListWindow;
        
        public RelayCommand ShowCustomerViewCommand { get; set; }
        public RelayCommand ShowInspectionViewCommand { get; set; }
        public RelayCommand ShowEventViewCommand { get; set; }
        public RelayCommand ShowInspectorViewCommand { get; set; }
        public RelayCommand ShowQuotationViewCommand { get; set; }
        public RelayCommand ShowQuestionnaireViewCommand { get; set; }


        public HomeViewModel()
        {
            _Context = new CustomFSContext();

            ShowCustomerViewCommand = new RelayCommand(ShowCustomerView);
            ShowInspectionViewCommand = new RelayCommand(ShowInspectionView);
            ShowEventViewCommand = new RelayCommand(ShowEventView);
            ShowInspectorViewCommand = new RelayCommand(ShowInspectorView);
            ShowQuotationViewCommand = new RelayCommand(ShowQuotationView);
            ShowQuestionnaireViewCommand = new RelayCommand(ShowQuestionnaireView);

            // Tests to make sure everything is working
            _Context = new CustomFSContext();
            ObservableCollection<CustomerVM> test = _Context.CustomerCrud.GetGetAllCustomerVMs();
            ObservableCollection<CustomerVM> test2 = _Context.CustomerCrud.GetFilteredCustomerBasedOnName("F");
            ObservableCollection<CustomerVM> test3 = _Context.CustomerCrud.GetCustomerById(51);

            // Place brakepoint here
            Console.WriteLine("");
        }

        private void ShowCustomerView()
        {
            new CustomerManagementView().Show();
        }

        private void ShowInspectionView()
        {
            throw new NotImplementedException();
        }

        private void ShowEventView()
        {
            new EventManagementView().Show();
        }

        private void ShowInspectorView()
        {
            throw new NotImplementedException();
        }

        private void ShowQuotationView()
        {
            throw new NotImplementedException();
        }

        private void ShowQuestionnaireView()
        {
            throw new NotImplementedException();
        }

    }
}