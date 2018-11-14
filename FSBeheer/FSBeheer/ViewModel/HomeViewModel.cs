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
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class HomeViewModel : ViewModelBase
    {
        private CustomFSContext _Context;

        public ObservableCollection<QuestionVM> Questions;

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

            //var questions = _Context.Questions.ToList().Select(q => new QuestionVM(q));
            //Questions = new ObservableCollection<QuestionVM>(questions);
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            ///

            ShowCustomerViewCommand = new RelayCommand(ShowCustomerView);
            ShowInspectionViewCommand = new RelayCommand(ShowInspectionView);
            ShowEventViewCommand = new RelayCommand(ShowEventView);
            ShowInspectorViewCommand = new RelayCommand(ShowInspectorView);
            ShowQuotationViewCommand = new RelayCommand(ShowQuotationView);
            ShowQuestionnaireViewCommand = new RelayCommand(ShowQuestionnaireView);


            var context = new CustomFSContext();
            ObservableCollection<CustomerVM> test = context.CustomerCrud.GetCustomerVMs;
        }

        private void ShowCustomerView()
        {
            _customerListWindow = new CustomerManagementView();
            _customerListWindow.Show();
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