﻿using FSBeheer.View;
using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class CreateEditCustomerViewModel : ViewModelBase
    {
        public CustomerVM Customer { get; set; }

        public RelayCommand<Window> SaveChangesCommand { get; set; }

        public RelayCommand<Window> DiscardCommand { get; set; }

        public RelayCommand<Window> DeleteCustomerCommand { get; set; }

        public RelayCommand CreateContactWindowCommand { get; set; }

        public RelayCommand EditContactWindowCommand { get; set; }

        public ObservableCollection<ContactVM> Contacts { get; set; }

        private ContactVM _selectedContact;
        public ContactVM SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                _selectedContact = value;
                base.RaisePropertyChanged(nameof(SelectedContact));
            }
        }

        private CustomFSContext _context;

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);
        public static bool IsInternetConnected()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        public CreateEditCustomerViewModel()
        {
            Messenger.Default.Register<bool>(this, "UpdateContactList", cl => UpdateCustomers()); // registratie, ontvangt (recipient is dit zelf) Observable Collection van CustomerVM en token is CustomerList, en voeren uiteindelijk init() uit, stap I

            UpdateCustomers();
            SaveChangesCommand = new RelayCommand<Window>(SaveChanges);
            DiscardCommand = new RelayCommand<Window>(Discard);
            DeleteCustomerCommand = new RelayCommand<Window>(DeleteCustomer);
            CreateContactWindowCommand = new RelayCommand(OpenCreateContact);
            EditContactWindowCommand = new RelayCommand(OpenEditContact);
        }

        internal void UpdateCustomers()
        {
            _context = new CustomFSContext();   
            if (Customer != null)
            {
                Contacts = _context.ContactCrud.GetContactByCustomer(Customer);
                RaisePropertyChanged(nameof(Contacts));
            }
        }

        private bool CustomerIsValid()
        {
            if (Customer.Address == null)
            {
                MessageBox.Show("Een klant moet een adres hebben.");
                return false;
            }

            if (Customer.Address.Trim() == string.Empty)
            {
                MessageBox.Show("Een klant moet een adres hebben.");
                return false;
            }

            if (Customer.ChamberOfCommerceNumber == null)
            {
                MessageBox.Show("Een klant moet een KVK nummer hebben.");
                return false;
            }

            if (Customer.City == string.Empty)
            {
                MessageBox.Show("Een klant moet een stad hebben.");
                return false;
            }

            if (Customer.City.Trim() == string.Empty)
            {
                MessageBox.Show("Een klant moet een stad hebben.");
                return false;
            }

            if (Customer.Name == null)
            {
                MessageBox.Show("Een klant moet een naam hebben.");
                return false;
            }

            if (Customer.Name.Trim() == string.Empty)
            {
                MessageBox.Show("Een klant moet een naam hebben.");
                return false;
            }

            if (Customer.StartingDate <= new DateTime(1990, 1, 1))
            {
                MessageBox.Show("De geselecteerde startdatum is incorrect.");
                return false;
            }

            if (Customer.ZipCode == null)
            {
                MessageBox.Show("Een klant moet een postcode hebben.");
                return false;
            }

            if (Customer.ZipCode.Trim() == string.Empty)
            {
                MessageBox.Show("Een klant moet een postcode hebben.");
                return false;
            }

            if (!Regex.Match(Customer.ZipCode, "^[0-9]{5}(?:-[0-9]{4})?$").Success)
            {
                MessageBox.Show("De ingevoerde postcode is incorrect.");
                return false;
            }

            return true;
        }

        private void OpenCreateContact()
        {
            if(IsInternetConnected())
                new CreateEditContactView(null, Customer).Show();
            else
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
        }

        private void OpenEditContact()
        {
            if (IsInternetConnected())
            {
                if (_selectedContact == null)
                {
                    MessageBox.Show("No contact selected");
                }
                else
                {
                    new CreateEditContactView(_selectedContact, Customer).Show();
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        public void SetCustomer(CustomerVM customer)
        {
            if (customer == null)
            {
                Customer = new CustomerVM();
                _context.Customers.Add(Customer.ToModel());
                RaisePropertyChanged(nameof(Customer));
            }
            else
            {
                Customer = new CustomerVM(_context.Customers.FirstOrDefault(c => c.Id == customer.Id));
                RaisePropertyChanged(nameof(Customer));
            }
            Contacts = _context.ContactCrud.GetContactByCustomer(Customer);
            RaisePropertyChanged(nameof(Contacts));
        }

        private void SaveChanges(Window window)
        {
            if (!CustomerIsValid()) return;

            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Save changes?", "Confirm action", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    Customer.StartingDate = DateTime.Today; 
                    _context.CustomerCrud.GetAllCustomers().Add(Customer);
                    _context.SaveChanges();
                    window.Close();

                    Messenger.Default.Send(true, "UpdateCustomerList"); // Stuurt object true naar ontvanger, die dan zijn methode init() uitvoert, stap II
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }

        private void Discard(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Close without saving?", "Confirm discard", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _context.Dispose();
                Customer = null;
                window.Close();
            }
        }

        /// <summary>
        /// Set Customer on Deleted, and also all contacts connected to it (fields)
        /// </summary>
        /// <param name="window"></param>
        private void DeleteCustomer(Window window)
        {
            if (IsInternetConnected())
            {
                MessageBoxResult result = MessageBox.Show("Delete the selected customer?", "Confirm Delete", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    Customer.IsDeleted = true;
                    foreach (var e in Contacts)
                    {
                        e.IsDeleted = true;
                    }
                    _context.SaveChanges();
                    window.Close();

                    Messenger.Default.Send(true, "UpdateCustomerList");
                }
            }
            else
            {
                MessageBox.Show("U bent niet verbonden met het internet. Probeer het later opnieuw.");
            }
        }
    }
}
