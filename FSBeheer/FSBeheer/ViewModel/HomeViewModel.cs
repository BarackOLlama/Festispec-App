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

        public RelayCommand ShowCustomersViewCommand { get; set; }
        public RelayCommand ShowInspectionViewCommand { get; set; }
        public RelayCommand ShowEventViewCommand { get; set; }
        public RelayCommand ShowInspectorsCommand { get; set; }
        public RelayCommand ShowQuotationsCommand { get; set; }
        public RelayCommand ShowQuestionnairesCommand { get; set; }


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

            ShowCustomersViewCommand = new RelayCommand(ShowCustomersView);
            ShowInspectionViewCommand = new RelayCommand(ShowInspectionsView);
            ShowEventViewCommand = new RelayCommand(ShowEventsView);
            ShowInspectorsCommand = new RelayCommand(ShowInspectorsView);
            ShowQuotationsCommand = new RelayCommand(ShowQuotationsView);
            ShowQuestionnairesCommand = new RelayCommand(ShowQuestionnairesView);


            var context = new CustomFSContext();
            ObservableCollection<CustomerVM> test = context.CustomerCrud.GetAllCustomerVMs;
            Console.WriteLine("");
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