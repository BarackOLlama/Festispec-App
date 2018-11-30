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

        public EditQuestionnaireViewModel EditQuestionnaireViewModel
        {
            get
            {
                _questionnaireViewModel = new EditQuestionnaireViewModel(_questionnaireManagementViewModel.SelectedQuestionnaire);
                return _questionnaireViewModel;
            }
        }

        public CreateQuestionViewModel NewQuestionCreationViewModel
        {
            get
            {
                return new CreateQuestionViewModel(_questionnaireManagementViewModel.SelectedQuestionnaire);
            }
        }

        public EditQuestionViewModel EditQuestion
        {
            get
            {
                return new EditQuestionViewModel(_questionnaireViewModel.SelectedQuestion);
            }
        }

        public CreateEditInspectionViewModel CreateEditInspection
        {
            get
            {
                return new CreateEditInspectionViewModel(_InspectionManagementViewModel);
            }
        }

        public AvailableInspectorViewModel AvailableInspector
        {
            get
            {
                return new AvailableInspectorViewModel();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}