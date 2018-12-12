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
        private EditQuestionnaireViewModel _questionnaireViewModel;
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

        public CreateEditInspectorViewModel CreateEditInspector
        {
            get
            {
                return new CreateEditInspectorViewModel();
            }
        }

        public CreateEditCustomerViewModel CreateEditCustomer
        {
            
            get
            {
                return new CreateEditCustomerViewModel();
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
                return new QuestionnaireManagementViewModel();
            }
        }

        public CreateEditContactViewModel CreateEditContact
        {
            get
            {
                return new CreateEditContactViewModel();
            }
        }

        public EditQuestionnaireViewModel EditQuestionnaireViewModel
        {
            get
            {
                _questionnaireViewModel = new EditQuestionnaireViewModel(_questionnaireManagementViewModel.SelectedQuestionnaire.Id);
                return _questionnaireViewModel;
            }
        }

        public CreateQuestionViewModel NewQuestionCreationViewModel
        {
            get
            {
                return new CreateQuestionViewModel(_questionnaireManagementViewModel.SelectedQuestionnaire.Id);
            }
        }

        public EditQuestionViewModel EditQuestion
        {
            get
            {
                return new EditQuestionViewModel(_questionnaireViewModel.SelectedQuestion.Id);
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

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}