using FSBeheer.VM;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using FSBeheer.View;
using GalaSoft.MvvmLight;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;

namespace FSBeheer.ViewModel
{
    public class CustomerManagementViewModel : ViewModelBase
    {
        private CustomFSContext CustomFSContext;
        public ObservableCollection<CustomerVM> Customers { get; set; }

        public RelayCommand EditCustomerWindowCommand { get; set; }

        public RelayCommand CreateCustomerWindowCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }

        public RelayCommand SearchTextCommand { get; set; }

        private CustomerVM _selectedCustomer { get; set; } 

        public CustomerVM SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                base.RaisePropertyChanged(nameof(SelectedCustomer));
            }
        }


        public CustomerManagementViewModel()
        {
            Messenger.Default.Register<bool>(this,"UpdateCustomerList", cl => Init()); // registratie, ontvangt (recipient is dit zelf) Observable Collection van CustomerVM en token is CustomerList, en voeren uiteindelijk init() uit, stap I

            Init();
            CreateCustomerWindowCommand = new RelayCommand(OpenCreateCustomer);
            EditCustomerWindowCommand = new RelayCommand(OpenEditCustomer);
            DeleteCommand = new RelayCommand(DeleteCustomer);
        }

        internal void Init()
        {
            CustomFSContext = new CustomFSContext();
            Customers = CustomFSContext.CustomerCrud.GetAllCustomerVMs();
            RaisePropertyChanged(nameof(Customers));
        }

        // Standard way of doing this
        private void OpenCreateCustomer()
        {
            new CreateEditCustomerView().Show();
        }
        private void OpenEditCustomer()
        {
            if (_selectedCustomer == null)
            {
                MessageBox.Show("No customer selected");
            }
            else
            {
                new CreateEditCustomerView(_selectedCustomer).Show();
            }
        }

        private void DeleteCustomer()
        {
            if (SelectedCustomer != null)
            {
                CustomFSContext.CustomerCrud.SetDeleted(SelectedCustomer);
                Customers.Remove(SelectedCustomer);
            }
        }
    }
}
