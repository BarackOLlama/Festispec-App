using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using CommonServiceLocator;

namespace FSBeheer.ViewModel
{
    public class ViewModelLocator
    {
        private QuestionnairesViewModel _questionnairesViewModel;
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<HomeViewModel>();
            _questionnairesViewModel = new QuestionnairesViewModel();
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
                return new CustomerManagementViewModel();
            }
        }

        //View of all questionnaires
        public QuestionnairesViewModel QuestionnairesViewModel
        {
            get
            {
                return _questionnairesViewModel;
            }
        }

        public QuestionnaireVM SelectedQuestionnaireVM
        {
            get
            {
                return _questionnairesViewModel.SelectedQuestionnaire;
            }
        }

        public CreateQuestionViewModel NewQuestionCreationViewModel
        {
            get
            {
                return new CreateQuestionViewModel(SelectedQuestionnaireVM);
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}