using FSBeheer.VM;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using FSBeheer.View;
using GalaSoft.MvvmLight;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using System.Runtime.InteropServices;
using System.Runtime.Caching;
using System;

namespace FSBeheer.ViewModel
{
    public class CustomerManagementViewModel : ViewModelBase
    {
        private CustomFSContext _context;
        public ObservableCollection<CustomerVM> Customers { get; set; }

        public RelayCommand<Window> CloseWindowCommand { get; set; }

        public RelayCommand EditCustomerWindowCommand { get; set; }

        public RelayCommand CreateCustomerWindowCommand { get; set; }

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

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        public CustomerManagementViewModel()
        {
            Messenger.Default.Register<bool>(this,"UpdateCustomerList", cl => FetchAndSetCustomers()); // registratie, ontvangt (recipient is dit zelf) Observable Collection van CustomerVM en token is CustomerList, en voeren uiteindelijk init() uit, stap I

            FetchAndSetCustomers();
            CreateCustomerWindowCommand = new RelayCommand(OpenCreateCustomer);
            EditCustomerWindowCommand = new RelayCommand(OpenEditCustomer);
            CloseWindowCommand = new RelayCommand<Window>(CloseWindow);
        }

        internal void FetchAndSetCustomers()
        {
            _context = new CustomFSContext();
            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddDays(1)
            };
            if (IsInternetConnected())
            {
                Customers = _context.CustomerCrud.GetAllCustomers();
                cache.Set("customers", Customers, policy);
            }
            else
            {
                Customers = cache["customers"] as ObservableCollection<CustomerVM>;
                if (Customers == null)
                {
                    Customers = new ObservableCollection<CustomerVM>();
                }
            }
            RaisePropertyChanged(nameof(Customers));
        }

        // Standard way of doing this
        private void OpenCreateCustomer()
        {
            if(IsInternetConnected())
                new CreateEditCustomerView().Show();
            else
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
        }

        private void OpenEditCustomer()
        {
            if (IsInternetConnected())
            {
                if (_selectedCustomer == null)
                {
                    MessageBox.Show("Geen klant geselecteerd.");
                }
                else
                {
                    new CreateEditCustomerView(_selectedCustomer).Show();
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        private void CloseWindow(Window window)
        {
            window.Close();
        }

        public void FilterList(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                Customers = _context.CustomerCrud.GetAllCustomers();
            }
            else
            {
                Customers = _context.CustomerCrud.GetAllCustomersFiltered(filter);
            }
            RaisePropertyChanged(nameof(Customers));
        }
    }
}
