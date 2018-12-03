using FSBeheer.VM;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FSBeheer.ViewModel
{
    public class CreateEditInspectorViewModel : ViewModelBase
    {

        public InspectorVM Inspector { get; set; }

        public InspectorVM SelectedInspector { get; set; }

        public RelayCommand SaveChangesCommand { get; set; }

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

        public CreateEditInspectorViewModel(InspectorVM SelectedInspector)
        {
            _Context = new CustomFSContext();
            SaveChangesCommand = new RelayCommand(SaveChanges);

            if (SelectedInspector != null)
            {
                Inspector = SelectedInspector;
            }
            else
            {
                Inspector = new InspectorVM()
                {

                };

            }
        }

        public CreateEditInspectorViewModel()
        {
            _Context = new CustomFSContext();
        }

        private void AddInspector()
        {

            _Context.InspectorCrud.GetAllInspectors().Add(Inspector);
            _Context.InspectorCrud.Add(Inspector);
        }


        //private void ModifyInspector()
        //{
        //    Console.WriteLine("ja");
        //    SelectedInspector.Name = Name;
        //    SelectedInspector.Address = Address;
        //    SelectedInspector.City = Inspector.City;
        //    SelectedInspector.Zipcode = Inspector.Zipcode;
        //    SelectedInspector.Email = Inspector.Email;
        //    SelectedInspector.PhoneNumber = Inspector.PhoneNumber;
        //    SelectedInspector.CertificationDate = Inspector.CertificationDate;
        //    SelectedInspector.InvalidDate = Inspector.InvalidDate;

        //    _Context.InspectorCrud.Modify(SelectedInspector);
        //}


        private void SaveChanges()
        {
            MessageBoxResult result = MessageBox.Show("Save changes?", "Confirm action", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _Context.InspectorCrud.GetAllInspectors().Add(Inspector);
                _Context.SaveChanges();

                Messenger.Default.Send(true, "UpdateInspectorList"); // Stuurt object true naar ontvanger, die dan zijn methode init() uitvoert, stap II
            }
        }

        public void SetInspector(InspectorVM inspector)
        {
            if (inspector == null)
            {
                Inspector = new InspectorVM();
                _Context.Inspectors.Add(Inspector.ToModel());
                RaisePropertyChanged(nameof(Inspector)); // a sign that a property has changed for viewing
            }
            else
            {
                Inspector = new InspectorVM(_Context.Inspectors.FirstOrDefault(c => c.Id == inspector.Id));
                RaisePropertyChanged(nameof(inspector));
            }
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

        private void Delete(Window window)
        {
            MessageBoxResult result = MessageBox.Show("Delete the selected customer?", "Confirm Delete", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                Inspector.IsDeleted = true;
                _Context.SaveChanges(); // TODO: Changes of last changes to customer stays, do we want that?
                window.Close();

                Messenger.Default.Send(true, "UpdateInspectorList");
            }
        }

    }
}
