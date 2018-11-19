using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using CommonServiceLocator;

namespace FSBeheer.ViewModel
{
    public class ViewModelLocator
    {
        private QuestionnaireListViewModel _questionListViewModel;
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
                return new CreateEditCustomerViewModel(CustomerManagement.SelectedCustomer);
            }
        }


        public LoginViewModel LoginViewModel
        {
            get
            {
                return new LoginViewModel(Home);
            }
        }

        public QuestionnaireListViewModel QuestionnaireList
        {
            get
            {
                _questionListViewModel = new QuestionnaireListViewModel();
                return _questionListViewModel;   
            }
        }

        public QuestionnaireViewModel SelectedQuestionnaireVM
        {
            get
            {
                return new QuestionnaireViewModel(_questionListViewModel.SelectedQuestionnaire.ToModel());
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