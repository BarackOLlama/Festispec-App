using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class EditInspectorViewModel : ViewModelBase
    {

        public InspectorVM Inspector { get; set; }

        public InspectorVM SelectedInspector { get; set; }

        public RelayCommand EditCommand { get; set; }

        public RelayCommand AddCommand { get; set; }

        public RelayCommand<Window> DiscardCommand { get; set; }

        private int _Id;
        public int Id { get { return _Id; } set { _Id = value; RaisePropertyChanged("Id"); } }
        private int _accountId;
        public int AccountId { get { return _accountId; } set { _accountId = value; } }
        private string _name;
        public string Name { get { return _name; } set { _name = value; RaisePropertyChanged("Name"); } }
        private string _address;
        public string Address { get { return _address; } set { _address = value; RaisePropertyChanged("address"); } }
        private string _residence;
        public string Residence { get; set; }
        private string _zipcode;
        public string Zipcode { get; set; }
        private string _email;
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? ValidDate { get; set; }
        public DateTime? InvalidDate { get; set; }
        public string BankNumber { get; set; }

        private CustomFSContext _Context;

        public EditInspectorViewModel(InspectorVM SelectedInspector)
        {
            _Context = new CustomFSContext();
            EditCommand = new RelayCommand(ModifyInspector);

            if (SelectedInspector != null)
            {
                this.SelectedInspector = SelectedInspector;
                Inspector = new InspectorVM();
                Id = SelectedInspector.Id;
                Name = this.SelectedInspector.Name;
                Address = this.SelectedInspector.Address;
                Residence = this.SelectedInspector.City;
                Zipcode = this.SelectedInspector.Zipcode;
                Email = this.SelectedInspector.Email;
                PhoneNumber = this.SelectedInspector.PhoneNumber;
                ValidDate = this.SelectedInspector.CertificationDate;
                InvalidDate = this.SelectedInspector.InvalidDate;
                BankNumber = this.SelectedInspector.BankNumber;
            }
            else
            {
                Inspector = new InspectorVM()
                {

                };

            }
        }

        public EditInspectorViewModel()
        {
            _Context = new CustomFSContext();
        }

        private void AddInspector()
        {

            _Context.InspectorCrud.GetAllInspectorVMs().Add(Inspector);
            _Context.InspectorCrud.Add(Inspector);
        }


        private void ModifyInspector()
        {
            SelectedInspector.Name = Name;
            SelectedInspector.Address = Inspector.Address;
            SelectedInspector.City = Inspector.City;
            SelectedInspector.Zipcode = Inspector.Zipcode;
            SelectedInspector.Email = Inspector.Email;
            SelectedInspector.PhoneNumber = Inspector.PhoneNumber;
            SelectedInspector.CertificationDate = Inspector.CertificationDate;
            SelectedInspector.InvalidDate = Inspector.InvalidDate;

            _Context.InspectorCrud.Modify(SelectedInspector);
        }

        private void Discard(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Close without saving?", "Confirm discard", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                _Context.Dispose();
                Inspector = null;
                window?.Close();
            }
        }

    }
}
