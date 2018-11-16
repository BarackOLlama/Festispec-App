using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using CommonServiceLocator;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class ViewModelLocator
    {
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
                if (CustomerManagement.SelectedCustomer != null)
                {
                    MessageBox.Show("You got a customer");
                    return new CreateEditCustomerViewModel(CustomerManagement.SelectedCustomer);
                } else
                {
                    MessageBox.Show("No customer");
                    return new CreateEditCustomerViewModel();
                }
            }
        }



        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}