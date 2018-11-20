using FSBeheer.Model;
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
        
        public AccountVM Account { get; set; }

        public ObservableCollection<QuestionVM> Questions;

        public RelayCommand ShowCustomerViewCommand { get; set; }
        public RelayCommand ShowInspectionViewCommand { get; set; }
        public RelayCommand ShowEventViewCommand { get; set; }
        public RelayCommand ShowInspectorViewCommand { get; set; }
        public RelayCommand ShowQuotationViewCommand { get; set; }
        public RelayCommand ShowQuestionnaireManagementViewCommand { get; set; }
        public RelayCommand ShowCreateEditViewCommand { get; set; }
        public RelayCommand ShowQuestionnairesViewCommand { get; set; }
        public RelayCommand ShowLoginViewCommand { get; set; }
        public RelayCommand ShowQuestionnaireListViewCommand { get; set; }

        public HomeViewModel()
        {
            _Context = new CustomFSContext();

            ShowCustomerViewCommand = new RelayCommand(ShowCustomerView);
            ShowInspectionViewCommand = new RelayCommand(ShowInspectionView);
            ShowEventViewCommand = new RelayCommand(ShowEventView);
            ShowInspectorViewCommand = new RelayCommand(ShowInspectorView);
            ShowQuotationViewCommand = new RelayCommand(ShowQuotationView);
            ShowLoginViewCommand = new RelayCommand(ShowLoginView);
            ShowQuestionnaireListViewCommand = new RelayCommand(ShowQuestionnaireListView);

            ShowCreateEditViewCommand = new RelayCommand(ShowCreateEditInspectionView);

            // Tests to make sure everything is working
            _Context = new CustomFSContext();
            ObservableCollection<CustomerVM> test = _Context.CustomerCrud.GetAllCustomerVMs();
            ObservableCollection<CustomerVM> test2 = _Context.CustomerCrud.GetFilteredCustomerBasedOnName("F");
            ObservableCollection<CustomerVM> test3 = _Context.CustomerCrud.GetCustomerById(51);

            // Place brakepoint here
            Console.WriteLine("");
        }

        private void ShowLoginView()
        {
            new LoginView().ShowDialog();
        }

        private void ShowCustomerView()
        {
            new CustomerManagementView().Show();
        }

        private void ShowInspectionView()
        {
            new InspectionManagementView().Show();
        }

        private void ShowEventView()
        {
            new EventManagementView().Show();
        }

        private void ShowInspectorView()
        {
            new InspectorManagementView().Show();
        }

        private void ShowQuotationView()
        {
            throw new NotImplementedException();
        }

        private void ShowQuestionnaireListView()
        {
            new QuestionnaireListView().ShowDialog();
        }

        private void ShowCreateEditInspectionView()
        {
           new EditInspectionView().Show();

        }
    }
}