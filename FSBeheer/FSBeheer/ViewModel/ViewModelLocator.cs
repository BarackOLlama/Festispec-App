using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using CommonServiceLocator;
using System;

namespace FSBeheer.ViewModel
{
    public class ViewModelLocator
    {
        private QuestionnaireManagementViewModel _questionListViewModel;
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<CustomerManagementViewModel>();
        }

        public HomeViewModel Home
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HomeViewModel>();
            }
        }

        public CustomerManagementViewModel CustomerManagement
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CustomerManagementViewModel>();
            }
        }

        public EventManagementViewModel EventManagement
        {
            get
            {
                return new EventManagementViewModel();
            }
        }

        public InspectionManagementViewModel InspectionManagement
        {
            get
            {
                return new InspectionManagementViewModel();
            }
        }

        public InspectorManagementViewModel InspectorManagement
        {
            get
            {
                return new InspectorManagementViewModel();
            }
        }

        public InspectorSelectionViewModel InspectorSelection
        {
            get
            {
                return new InspectorSelectionViewModel();
            }
        }

        public CreateEditCustomerViewModel CreateEditCustomer
        {
            
            get
            {
                throw new NotImplementedException();
                //return new CreateEditCustomerViewModel(CustomerManagement.SelectedCustomer);
            }
        }

        public CreateQuestionnaireViewModel CreateQuestionnaire
        {
            get
            {
                return new CreateQuestionnaireViewModel();
            }
        }


        public LoginViewModel LoginViewModel
        {
            get
            {
                return new LoginViewModel(Home);
            }
        }

        public QuestionnaireManagementViewModel QuestionnaireList
        {
            get
            {
                _questionListViewModel = new QuestionnaireManagementViewModel();
                return _questionListViewModel;   
            }
        }

        public QuestionnaireViewModel EditQuestionnaireViewModel
        {
            get
            {
                return new QuestionnaireViewModel(_questionListViewModel.SelectedQuestionnaire);
            }
        }

        public CreateQuestionViewModel NewQuestionCreationViewModel
        {
            get
            {
                return new CreateQuestionViewModel(_questionListViewModel.SelectedQuestionnaire);
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}