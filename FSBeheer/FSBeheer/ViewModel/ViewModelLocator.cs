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
        }

        public HomeViewModel Home
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HomeViewModel>();
            }
        }

        public CreateEditCustomerViewModel CreateEditCustomer
        {
            get
            {
                return new CreateEditCustomerViewModel();
            }
        }

        public CustomerManagementViewModel CustomerManagement
        {
            get
            {
                return new CustomerManagementViewModel();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}