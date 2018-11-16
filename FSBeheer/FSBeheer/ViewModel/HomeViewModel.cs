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
        private FSContext _Context;

        public ObservableCollection<QuestionVM> Questions;

        private CustomerListWindow _customerListWindow;
        private CreateEditCustomerView _createEditInspectionWindow;

        public RelayCommand ShowCustomerViewCommand { get; set; }
        public RelayCommand ShowInspectionViewCommand { get; set; }
        public RelayCommand ShowEventViewCommand { get; set; }
        public RelayCommand ShowInspectorCommand { get; set; }
        public RelayCommand ShowQuotationCommand { get; set; }
        public RelayCommand ShowQuestionnairCommand { get; set; }

        public RelayCommand ShowCreateEditCommand { get; set; }

        public HomeViewModel()
        {
            _Context = new FSContext();

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
            ShowInspectorCommand = new RelayCommand(ShowInspectorView);
            ShowQuotationCommand = new RelayCommand(ShowQuotationView);
            ShowQuestionnairCommand = new RelayCommand(ShowQuestionnairView);

            ShowCreateEditCommand = new RelayCommand(ShowCreateEditInspectionView);


            var context = new CustomFSContext();
            ObservableCollection<CustomerVM> test = context.CustomerCrud.GetCustomerVMs;

           
        }


        private void ShowCustomerView()
        {
            _customerListWindow = new CustomerListWindow();
            _customerListWindow.Show();
        }

        private void ShowInspectionView()
        {
            throw new NotImplementedException();
        }

        private void ShowEventView()
        {
            throw new NotImplementedException();
        }

        private void ShowInspectorView()
        {
            throw new NotImplementedException();
        }

        private void ShowQuotationView()
        {
            throw new NotImplementedException();
        }

        private void ShowQuestionnairView()
        {
            throw new NotImplementedException();
        }

        private void ShowCreateEditInspectionView()
        {
            _createEditInspectionWindow = new CreateEditCustomerView();
            _createEditInspectionWindow.Show();
        }
    }
}