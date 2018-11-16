using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using CommonServiceLocator;

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
      
        public CreateEditCustomerViewModel CreateEditCustomer
        {
            get
            {
                if (CustomerManagement.SelectedCustomer != null)
                {
                    return new CreateEditCustomerViewModel(CustomerManagement.SelectedCustomer);
                } else
                {
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