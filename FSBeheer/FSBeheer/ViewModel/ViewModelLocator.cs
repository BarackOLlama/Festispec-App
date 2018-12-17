using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using CommonServiceLocator;
using System;
using FSBeheer.VM;
using System.Collections.ObjectModel;

namespace FSBeheer.ViewModel
{
    public class ViewModelLocator
    {
        private QuestionnaireManagementViewModel _questionnaireManagementViewModel;
        private CreateEditQuestionnaireViewModel _questionnaireViewModel;
        private InspectorManagementViewModel _inspectorManagementViewModel;
        private InspectionManagementViewModel _InspectionManagementViewModel;

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

        public CreateEditEventViewModel CreateEditEvent
        {
            get
            {
                return new CreateEditEventViewModel();
            }
        }

        public InspectionManagementViewModel InspectionManagement
        {
            get
            {
                _InspectionManagementViewModel = new InspectionManagementViewModel();
                return _InspectionManagementViewModel;
            }
        }

        public InspectorManagementViewModel InspectorManagement
        {
            get
            {
                _inspectorManagementViewModel = new InspectorManagementViewModel();
                return _inspectorManagementViewModel;
            }
        }

        public CreateEditInspectorViewModel CreateEditInspector
        {
            get
            {
                return new CreateEditInspectorViewModel(_inspectorManagementViewModel.SelectedInspector);
            }
        }

        public CreateEditCustomerViewModel CreateEditCustomer
        {
            
            get
            {
                return new CreateEditCustomerViewModel();
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
                _questionnaireManagementViewModel = new QuestionnaireManagementViewModel();
                return _questionnaireManagementViewModel;   
            }
        }

        public CreateEditContactViewModel CreateEditContact
        {
            get
            {
                return new CreateEditContactViewModel();
            }
        }

        public CreateEditQuestionnaireViewModel CreateQuestionnaire
        {
            get
            {
                //create
                return new CreateEditQuestionnaireViewModel();
            }
        }

        public CreateEditQuestionnaireViewModel EditQuestionnaire
        {
            get
            {
                //edit
                _questionnaireViewModel = new CreateEditQuestionnaireViewModel(_questionnaireManagementViewModel.SelectedQuestionnaire.Id);
                return _questionnaireViewModel;
            }
        }

        public CreateEditQuestionViewModel CreateQuestion
        {
            get
            {
                //create, questionnaire id
                return new CreateEditQuestionViewModel(_questionnaireViewModel.Questionnaire.Id);
            }
        }

        public CreateEditQuestionViewModel EditQuestion
        {
            get
            {
                //edit, questionnaire id and question id
                return new CreateEditQuestionViewModel(_questionnaireViewModel.SelectedQuestion.Id, _questionnaireViewModel.SelectedQuestion.Id);
            }
        }

        public AvailableInspectorViewModel AvailableInspector
        {
            get
            {
                return new AvailableInspectorViewModel();
            }
        }

        public CreateEditInspectionViewModel CreateEditInspection
        {
            get
            {
                return new CreateEditInspectionViewModel();
            }
        }

        public BusinessDataViewModel BusinessData
        {
            get
            {
                return new BusinessDataViewModel();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}