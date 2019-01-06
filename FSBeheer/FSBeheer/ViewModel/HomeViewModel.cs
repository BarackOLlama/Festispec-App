using FSBeheer.Model;
using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private CustomFSContext _context;
        private AccountVM _account;
        public AccountVM Account
        {
            get { return _account; }
            set
            {
                _account = value;
                base.RaisePropertyChanged(nameof(Account));
                AccountRole = _context.Roles.ToList().Where(e => e.Id == Account.RoleId).FirstOrDefault().Content;
            }
        }
        public DateTime LoginTime { get; set; }
        public string AccountRole { get; set; }

        public ObservableCollection<QuestionVM> Questions;

        public RelayCommand ShowCustomerViewCommand { get; set; }
        public RelayCommand ShowInspectionViewCommand { get; set; }
        public RelayCommand ShowEventViewCommand { get; set; }
        public RelayCommand ShowInspectorViewCommand { get; set; }
        public RelayCommand ShowQuotationViewCommand { get; set; }
        public RelayCommand ShowQuestionnaireManagementViewCommand { get; set; }
        public RelayCommand ShowCreateEditViewCommand { get; set; }
        public RelayCommand ShowBusinessDataViewCommand { get; set; }

        public HomeViewModel(AccountVM account)
        {
            _context = new CustomFSContext();
            _account = account;
            base.RaisePropertyChanged(nameof(Account));
            ShowCustomerViewCommand = new RelayCommand(ShowCustomerView);
            ShowInspectionViewCommand = new RelayCommand(ShowInspectionView);
            ShowEventViewCommand = new RelayCommand(ShowEventView);
            ShowInspectorViewCommand = new RelayCommand(ShowInspectorView);
            ShowQuotationViewCommand = new RelayCommand(ShowQuotationView);
            ShowCreateEditViewCommand = new RelayCommand(ShowCreateEditInspectionView);
            ShowQuestionnaireManagementViewCommand = new RelayCommand(ShowQuestionnaireManagementView);
            ShowBusinessDataViewCommand = new RelayCommand(ShowBusinessDataView);

            LoginTime = DateTime.Now;

            // Tests to make sure everything is working
            //_context = new CustomFSContext();
            //ObservableCollection<CustomerVM> test = _context.CustomerCrud.GetAllCustomers();
            //ObservableCollection<CustomerVM> test2 = _context.CustomerCrud.GetFilteredCustomerBasedOnName("F");
            // ObservableCollection<CustomerVM> test3 = _context.CustomerCrud.GetCustomerById(51);

            // Place brakepoint here
            Console.WriteLine("");
            //this.ShowLoginView();
        }

        private void ShowBusinessDataView()
        {
            MessageBox.Show("Account username:"+Account.Username+"\nIngelogd sinds:"+LoginTime+"\nFunctie"+AccountRole);
            //new BusinessDataView().ShowDialog();
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

        private void ShowCreateEditInspectionView()
        {
            new CreateEditInspectionView().Show();
        }
        private void ShowQuestionnaireManagementView()
        {
            new QuestionnaireManagementView().ShowDialog();
        }
    }
}